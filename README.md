### About The Project


This was a small game I was developing back in 2022. As a hobby. Written in 3 spare weekends. It's a dialog game. Like those old school text-based games.


I'm putting it here to showcase a bit of my coding style. Since most of my code was developed for clients, I can't upload it publically on Github, for obvious reasons. It's the case for many good developers I know. It goes to show.


It was originally developed in GameMaker 2. But I migrated to Dotnet because it was more about dialogs rather than visuals, so using a typed language like Dotnet made more sense.
<br/>


---


### Context


If you've ever played this sort of dialogue-based game, you know its problem. One choice leads to another 3. And 3 more. And so on. In programming terms that means you just gave your writer an O(n!) number of dialogs to write. Not a good thing.


So I started coding this framework to solve that. How to enable someone to write a text that is natural. That looks natural. Those have a rhythm. Pauses. Thoughts come, and go away. Things change. With context or just over time. That all has to be modeled, somehow.


Here's the code on some of those folders help solve this problem:


> \- **Control** is just a basic command window MVVC engine. This pattern is often used for software with asynchronous interaction, like video games, mobile. Even front-end in general.
>
> \- **DialogBase** is a conversation engine, modeled as a directed graph. You're always at multiple nodes. Every interaction is an edge you traverse. An edge can be a message sent. A message received. A timeout. Information you just learned. The flow that a normal conversation has translates into edges that appear, disappear, change direction in the graph.
>
> \- **DialogBuilderBase** is a Linq-style language to build those graphs above. At the end of the day, an actual person has to write the texts. Even if 1000 monkeys could write Shakespeare, they would need to ask him to know. It's made to be extensible, but the more it is, the more about software development the writers need to know. There's a tradeoff here. You wouldn't want to waste much of Shakespeare's time learning python, although that would make for a great theater play.
>
> \- **Dialogs** is a sample of that process. It's a small dialog about pets written by me. You can see how in that simple example it's already a lot of writing. This is me doing a bit of the work the writers would have, so I can feel their problems and build the engine with them in mind.
>
> \- **Infos** models the context of one person about who they're talking to. If I know you dog is named 'Buddy' or 'Duke' I can say 'Is Buddy ok?' instead of 'Is your dog ok?'. It allows the writer to write a single text with adaptable context, instead of 3. And to add flavor text here and there. Those small, thoughtful, comments that make actual conversations sharp, and interesting. A dog joke that works with any name is possible, but wouldn't be as funny. 'Buddy is my buddy!' lands. 'Duke is my buddy' not so much. Just to give an example.
>
> \- **Options** are the messages the player can send to who he's talking to at the moment. The trick here is that you have, at most, only 3 options available. This limitation is intentional. Your player only has a certain amount of headspace. But those 3 options change. A lot. They appear, disappear, change, take time. Some have only a short margin of time, and if you don't take it, it's gone forever. If you're paying attention, real conversations have a very similar ruleset.
>
> \- **imgs** is just some schematics of how I thought of the process. It was made by me, for me. I'm putting it there just if you're curious.


<br/>


---


### History


This came from a discussion with a friend about dating apps. Of getting ghosted. Not getting a reply. So we though:


> *What if: Tinder, but even more stupid? What if we took this problem, and solved it in a way that created another, way worse?
> What if: if you don't reply to a message in 30 seconds or so, that person is gone, forever.*


It's a stupid idea, of course. So we gave it a stupid name. Something like Tinder, but more fickle? We came up with the name **Matchstick**. If that's a copyright infringement it wasn't at the time.


Here's a brief, ironic, description of **Matchstick**:


> Matchstick is the new dating app here to solve all your problems.
>
> Matchstick uses advanced AI, built with a laser-focus to solve the biggest problem with the current dating app ecosystem:
> <br/>
>
> - Tired of not getting answers?
>   Our Flame Kindling ™ adaptive learning tech makes sure you always get an answer straight away.
> - Tired of people that look like carbon-copy of each other?
>   Our Match Making ™ decision-tree algorithm will connect you to people of all walks of life. What you need may not  be what you want, and someone out there will be ready to teach you.
> - Tired of creeps?
>   Our Heartbreak Firewall ™ uses top-tier natural language processing to make 100% sure you will not reveal any unwanted personal information, even by accident.
> - Tired missing hidden social checks?
>   Our Fire Stoker ™ deep learning tech can help you with that, too. Maybe all you're missing is that tiny nudge to make your personality shine.
>   <br/>
>
> Here our testimonials:
>
> *"I always thought I was broken, but all it took was one extra push to make myself shine. I still need to figure out how to translate it to real life, but I'm not at that point yet."*
>
> *"Yeah, it's kinda cool."*
>
> *"Turns out dating in the uncanny valley can be a loads of fun."*
>
> *"Finally something with no in-app purchases! I wonder what their business model is?"*
>
> *"I used to be lonely and miserable. I thought no one could get me. Matchstick connected to all sorts of people out there and made me learn so much about them and myself. I'm still lonely, but I got a new appreciation for how many different types of misery there's in the world."*
>
> *"I met my boyfriend on Matchstick. He's fine"*
>
> Make friends, socialize, find yourself and maybe meet that special someone in this new bright example of technology used for good.


That was all just months before the explosion of GenAI technology. A bit of a coincidence but not as much. I believe that's what living in the Zeitgeist means. Either that or I can predict the future. Still in doubt of which.
<br/>


---


### License


Feel free to use this code. I'm most likely won't ever finish it, so put it to good use if you can. The code or the ideas behind it. Partially or even in full. The MIT License is there for this reason.


If you do, let me know. Don't want any royalties. Just bragging rights.







