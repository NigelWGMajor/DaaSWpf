# DaasWpf

## Dad as a Service
### (Windows Presentation Foundation MVVM version)

|IDE|DotNet|Version
|-|-|-|
|VS 2017|4.5|0.1.x.x

This is based on the following code challenge:

> Create an application using C#, .NET, and optionally ASP.NET that uses the
> [I can haz dad joke api](https://icanhazdadjoke.com/api) 
> to display jokes.
>
> You are welcome to use more technologies like Angular if you wish but it's not required.
>
> There should be two modes the user can choose between:
> 
> 1. Display a random joke every 10 seconds.
> 2. Accept a search term and display the first 30 jokes containing that term, 
>   with the matching term emphasized in some way (upper, bold, color, etc.) 
>   and the matching jokes grouped by length: short (<10 words), medium (<20 words), long (>= 20 words).

I used this as an opportunity to integrate some of what I have learned recently in the Angular/Node environment using NgRx for the Redux pattern: the concepts of Store and Reducers already parallel the WPF MVVM pattern, and WPF's ICommand interface can behave much like the Actions in the angular environment.  What I was missing was the clear definition of the boundary between synchronous and asynchronous realms that Reducers and Effects offer: I was very much wanting to try something similar in an MVVM setting.

In this solution, the Model has only one entry point:  Requests are "pushed" from the ViewModel into the Model, which returns the calling thread quickly after submitting the pending asynchronous action to the system's background worker stack.  The requested action is later executed on a background thread and ultimately resolves by invoking a success action carrying the resultant object, or a failure action carrying an exception object.

- If an exception occurs, this would happen  on the background worker thread, so we defer handling this until the final completion handler so that the exception handler can be invoked on the UI thread;
- Operations successfully returning the  resultant are likewise invoked on the UI thread.

In a manner somewhat reminiscent of Functional Programming, we pass the Success and Failure actions to the handlers using Lambda statements. Because these are defined in the context of the ViewModel, we have access to the members of the ViewModel without needing to create other references or special events.

By deriving different requestxxx classes from an abstract Request class in the Model, we allow arbitrary parameters to be passed to the background process with full intellisense and type safety. The return type for a successful action is established by using generics in the push call.

Although the internals are fairly complex, as seen from the ViewModel, the calls are clean and simple, which is the whole intent.  For example, the search API is called using this call:

```c3s
100 private void DoSearch(object o)
101 {
102     _model.Push<List<Joke>>(
103        new DaaSModel.RequestFind(
104            SearchTerm, 
105            c => Jokes = c, 
106            e => Error = e));
107 }
```

Notes:

- line 100: We don't use the object, but it could have been passed from the xaml environment as a parameter;
- line 102: The generic ```<List<Joke>>``` defines the result type of the successful operation;
- line 103: The ```RequestFind``` class is derived from the abstract ```Request``` class so that input parameters can be strongly typed;
- line 104: ``` SearchTerm``` is a bound ViewModel property passed as a parameter;
- line 105: This lambda simply sets a property in the ViewModel.  ```c``` will contain an object of the generically specified type;
- line 106: ```e``` will be an ```Exception```. Here it is simply passed to a property in the ViewModel.

When I use this technology again in a serious application I will refactor the base functionality in the model into a support class that can simply be subclassed in a project. At that stage I'll update this repo.