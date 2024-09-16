# What is SimpLog.Email
Simple and very flexible library tool for .NET Core. Gives opportunity to receive logs via email with the proper configuration.

# Log Types in SimpLog
| Type | Description |
| ----- | ----- |
| Trace | This should be used during development to track bugs, but never committed to your VCS. |
| Debug | Log at this level about anything that happens in the program. This is mostly used during debugging, and I’d advocate trimming down the number of debug statement before entering the production stage, so that only the most meaningful entries are left, and can be activated during troubleshooting. |
| Info | Log at this level all actions that are user-driven, or system specific (ie regularly scheduled operations…) |
| Notice | This will certainly be the level at which the program will run when in production. Log at this level all the notable events that are not considered an error. |
| Warn | Log at this level all events that could potentially become an error. For instance if one database call took more than a predefined time, or if an in-memory cache is near capacity. This will allow proper automated alerting, and during troubleshooting will allow to better understand how the system was behaving before the failure. |
| Error | Log every error condition at this level. That can be API calls that return errors or internal error conditions. |
| Fatal | Too bad, it’s doomsday. Use this very scarcely, this shouldn’t happen a lot in a real program. Usually logging at this level signifies the end of the program. For instance, if a network daemon can’t bind a network socket, log at this level and exit is the only sensible thing to do. |

# Features of SimpLog.Email

| Features | Description |
| ----- | ----- |
| &#128231; Email notifications | With SimpLog.Email you can also send emails with the logs. |

# Configuration

**In Program.cs**
```
Nothing needed
```

**In Controller**

```
private SimpLog.Email.Services.SimpLogServices.SimpLog _simpLog = new SimpLog.Email.Services.SimpLogServices.SimpLog();

```

and call the log like
```
_simpLog.Trace("place your message here");
```

options are as follows
```
_simpLog.Info({1}, {2}); 
```
and only {1} is required

| Option | Short Description | Full Description |
| ----- | ----- | ----- |
| {1} | Message | The message you want to log. |
| {2} | Send Email | If it is set to false the email notifications will be disabled only for this instance. If null or true, depending on the appsettings.json file EmailConfiguration section. |

**In simplog.json**

Create simplog.json file in the root folder of your startup project. On the same level where is appsettings.json. Please have in mind that every configuration in simplog.json is optional ☺️

An example for simplog.json can be found in folder samples.

```
{
    "Email_Configuration": {                -> Email configuration.
      "SendEmail_Globally": bool,           -> Field to disable sending emails globally. Default value is true.
      "Email_From": string,                 -> Who will be the sender of your emails.
      "Email_To": string,                   -> Who will be the recipient of the emails.
      "Email_BCC": string,                  -> If you want, you can add blind copy.
      "Enable_SSL": bool,                   -> Enable or disable ssl
      "Email_Connection": {                 -> Email configuration to the email service provider.
        "Host": string,
        "Port": string,
        "API_Key": string,
        "API_Value": string
      }
    },
    "LogType": {
      "Trace": {                            -> TYPE OF LOG == Trace.
        "SendEmail": true,                  -> For the TYPE OF LOG, should be enabled or disabled sending emails. Default value is true.
      },
      "Debug": {                            -> Analogically TYPE OF LOG here is Debug 
        "SendEmail": true,
      },
      "Info": {                             -> Analogically TYPE OF LOG here is Info
        "SendEmail": true,
      },
      "Notice": {                           -> Analogically TYPE OF LOG here is Notice
        "SendEmail": true,
      },
      "Warn": {                             -> Analogically TYPE OF LOG here is Warn
        "SendEmail": true,
      },
      "Error": {                            -> Analogically TYPE OF LOG here is Error
        "SendEmail": true,
      },
      "Fatal": {                            -> Analogically TYPE OF LOG here is Fatal
        "SendEmail": true,
      }
  }
  ```

Hope you enjoy the NuGet Package! 😉