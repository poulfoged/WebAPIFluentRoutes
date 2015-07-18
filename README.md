# WebAPIFluentRoutes
Finds routes in WebAPI without magic strings using lambda expressions 

## Example

To find a url inside a WebAPI controller method you must pass in current ControllerContext, the controller type and a lambda that (1) points to the right method to link to. (2) provides the parameters to add to the method call:

```c#
// given the action:
[HttpGet, Route("")]
public string Get(int number) { /*...*/ }

// find the route
Uri url = new RouteFinder().Link<Controller>(ControllerContext, c => c.Get(10)

// url will now be http://localhost?number=10
				
```

See [DummyController.cs](/source/WebAPIFluentRoutes.WebTest/Controllers/DummyController.cs) for more examples.

<!--## How to

Simply add the Nuget package:

`PM> Install-Package WebAPIFluentRoutes`-->

## Requirements

You'll need .NET Framework 4.5.2 or later to use the precompiled binaries.

## License

WebAPIFluentRoutes is under the MIT license.
