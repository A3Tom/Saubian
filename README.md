# Saubian #

A cosmic jaunt into the Azure underworld, slingin sanguine Siberian booty callers' emails into a service bus n out to an Alexa. Read me ma romantic Ruski requests

The goal of this project is to scan my inbox for some sexy wee Russian smashin messages into ma spam emails and then have them read out by Alexa in an array of voices at random intervals throughout the day or on command 


---
## Background : ##
This idea has been knockin about for a while but it collided with me investagting Azure functions and aw the mad hings that Azure has in it's bowels. So I was wanting to combine all these elements into one program as a primer / example / the noobgains from failing and learning before going onto larger projects

That was one element but another was I just wanty make Alexa say shit, I've got other projects kickin about ma heed for other hings too about Alexa but I hadn't messed about with the Alexa dev shit before and I remember absolutely pummelling those mad TTS shitty voice hings to make them scream obscenities in their mad robotic overlord voice. Fuckin incredible times.

So this is Azure x Alexa : A Cosmic Jaunt 

---
## Concept : ##
A function app will operate on a timer to poll a specific set of folders within my mail account. These folders will be able to be added to or removed from. Ideally I'll have an admin control panel for this too but lets not get ahead of ourselves here. This function app will send basic details about the email to a service bus queue topic. The emails retreived will be within a specific timeframe from the moment of query to not keep retrying and readding duplicate mail items

These emails will then be consumed by a second function app to be cleansed - triggered by the message queue. Post-cleansing; a new messaged will be raised on the service bus with a flag to denote the message has been cleansed and is ready for processing.

A third function app will then be triggered by a message queue will then pick this up and send it along to Alexa. Pending the success of that action the message may be consumed, resent or if a significant number of these have failed then DLQ'd

---
## Implementation : ##

MVP : 

An API, core domain model and 3 function apps; one timer and two MQ based triggers processing emails in a specified folder and then shouting them out in any way via Alexa

---
## Bonus Points : ##
Storage account to allow words / phrases to be stored for replacement in sentences. To allow aliasing and common words to be pronounced better by Alexa

Improved cleansing of messages - where is the 80/20 boundary on this one before we have to bite the bullet and go for a NN based cleansing scheme (how fuckin class does that sound though. Wit a mad element to add to this whole hing, like bumpin it up a dimension, this could be an insane shout)

More than one Alexa voice - even better when different accents are involved

Admin control center GUI; in any form would be nice to have

Logging of prior messages 

Retrieve prior messages on command

---
## Super Bonus Points : ##

IT IS AN INSANE SHOUT. NN Based message cleansing scheme. This would be a fuckin slam dunk as far as Super Bonus Points goes. Fuckin hyyyype if a get onto doing this


## Architecture ##
---

<img src="src\Architecture.svg">

---
## Credits : ##
As much as I would absolutely love to say "here mate am the best in the world at this shit", realistically this will - as per every project ever - be the product of a whole load of resource gathering n playdough-ing it into an angular project. As is the way.

This is a log of items watched or read which I've then implemented and bastardised for future reference.

```csharp OrderBy
return await creditList
    .OrderBy(x => x.Type.ArbitraryWeightValueThatIMadeUpToFormatTheTable)
    .ThenBy(x => x.TimeAccessedRelativeToProject)
    .ToListAsync();
```

|Type|Author|Gist|Areas Covered|Link|
|---|---|---|---|:---:|
|Documentation|Microsoft|Durable Function Chaining|Durable Functions|[Cheers Mate](https://docs.microsoft.com/en-us/azure/azure-functions/durable/durable-functions-sequence?tabs=csharp)|
|Article|John Staughton|How pigeon post worked|Homing Pigeons|[Cheers Mate](https://www.scienceabc.com/eyeopeners/how-did-the-pigeon-post-work.html)|
|Article|Daniel Krzyczkowski|Integrate Key Vault Secrets With Azure Functions|Azure Functions, AzureKeyVault|[Cheers Mate](https://daniel-krzyczkowski.github.io/Integrate-Key-Vault-Secrets-With-Azure-Functions/)|
|Article|Steve Gordon|Sending Json using HttpClient|Http Client, Json Serialization|[Cheers Mate](https://www.stevejgordon.co.uk/sending-and-receiving-json-using-httpclient-with-system-net-http-json/)|

