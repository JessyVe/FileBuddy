# FileBuddy
**EDUCATION Repository**

This Repository is part of an educational course of University of Applied Science -  [FH JOANNEUM GmbH](https://www.fh-joanneum.at/iit).

Bachelor program:

-   [Mobile Software Development]([https://www.fh-joanneum.at/mobile-software-development/bachelor/](https://www.fh-joanneum.at/mobile-software-development/bachelor/))  (FH JOANNEUM)

**Course:**

-  Web Service Development

## Introduction
In course of this project, a file sharing platform was implemented, which enables the easy transfer of various files between end devices.

The following implementations are planned/done:
- [x] Web API (C# .Net Core 3)
- [ ] Web socket server
- [x] WPF application
- [ ] Native Kotlin Android App

**Application Modules:**

-  Backend: [Web API]([https://github.com/JessyVe/FileBuddy/tree/master/BackendImplementation](https://github.com/JessyVe/FileBuddy/tree/master/BackendImplementation))
-  Frontend: [WPF application]([https://github.com/JessyVe/FileBuddy/tree/master/FrontendImplementation](https://github.com/JessyVe/FileBuddy/tree/master/FrontendImplementation))

## Execution of the project
The project was developed in the [Visual Studio 19 Community Edition]([https://visualstudio.microsoft.com/de/vs/](https://visualstudio.microsoft.com/de/vs/)). In order to start the application for testing purposes (API + UI) do the following:

1. Open the solution file in VS 19 *(relative path: FileBuddy\BackendImplementation\FileBuddyBackendFileBuddyBackend.sln)*
2. Click right on the display solution 'FileBuddyBackend' *(default: right hand side of the screen)* and open the **Properties** window.
3. Activate the **Multiple startup projects:** radiobutton and select **Start** at the actions for the projects *API* and *FileBuddyUI*.
4. Press **Apply** and **OK**
5. Now, press the **Start** button on the top-middle of the window.

After this, the follwing windows should open (mind that building process might take a long at the first time):
- Console of API with start up information
- [Desktop client](https://github.com/JessyVe/FileBuddy/tree/master/FrontendImplementation)
- Brower window (default browser) with [Swagger UI](https://swagger.io/tools/swagger-ui/) API documentation
