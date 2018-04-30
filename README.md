# Overwatch League Chatbot [![Build Status](https://travis-ci.org/lfmundim/OWLeagueChatbot.svg?branch=master)](https://travis-ci.org/lfmundim/OWLeagueChatbot)

### Developed by:
* [Guilherme Vieira](https://github.com/guixvieira)
* [Lucas Mundim](https://www.github.com/lfmundim)
* [Pedro Nascimento](https://github.com/PedroPrisxzma)
* [Sonia Fraga](https://github.com/soniaAlejandra)
as part of a class assignment.

### Architecture:
* Developed using `.NET Core 2.0` (and [BLiP's SDK](https://docs.blip.ai/#c))
* Integration with Blizzard's *Overwatch League* [API](api.overwatchleague.com) - methods scrubbed by [this reddit post](https://www.reddit.com/r/Competitiveoverwatch/comments/7p0e8d/owl_api_analysis/)
* Channel Integration using [BLiP](preview.blip.ai)
* Dependency injection using `Simple Injector`, including Singleton registry

### Overview of BLiP's Extensions and their uses
BLiP's own extensions (such as `ISender` and `ISchedulerExtension`) basically send `HTTP Requests` to `IRIS`, BLiP's host. It acts like a `gateway` to route messages to wherever they are sent to, or commands for that matter (ex: IBM's *Watson* integration).

#### Scheduler
Example HTTP Request

```
POST https://msging.net/commands HTTP/1.1
Content-Type: application/json
Authorization: Key {YOUR_TOKEN}

{  
  "id": "1",
  "to": "postmaster@scheduler.msging.net",
  "method": "set",
  "uri": "/schedules",
  "type": "application/vnd.iris.schedule+json",
  "resource": {  
    "message": {  
      "id": "ad19adf8-f5ec-4fff-8aeb-2e7ebe9f7a67",
      "to": "destination@0mn.io",
      "type": "text/plain",
      "content": "Scheduling test"
    },
    "when": "2016-07-25T17:50:00.000Z"
  }
}
```
end