# EmailUtility
Sending email using .net 5 console app and razor pages as template along with logging via log4net. 
<br />
<br />
Just get your model from anywhere and add it into your .cshtml template.
<br />
This app is using nuget package RazorEngineCore for creating the template. 
<br />
Logging has been added using dependency injection. 
<br />
In Logging, there are 2 appenders, first is for logging everything into the log files while other is for sending emails on error. 
<br />
<br />
Please note- Templates are not html safe by default but you can use this [link](https://github.com/adoconnection/RazorEngineCore/wiki/@Raw) to make them.
