DISCLAIMER
==========
This is a portion of a POC code that I wrote in (I'd say) 2009. It demoes the
use of impersonation to get user-specific information.

Note:  that most likely, in the age of Win64 you'd have to slightly do other 
things to get there.

Code written by myself, in my spare time on my own hardware - no work code was 
used to demo Impersonation.

Introduction
============
Get handle and pid, will impersonate. Once again - most likely this won't
work on newer Windows versions.

History
==========

Prelude:

We had a product based on RedMon (GhostScript/printer driver) that allowed
some portions to be printed to a PDF file. It worked pretty good - and then
it was handed over to someone within the company who
* had a hard time communicating
* added crazy features to the installer (spinning hourglass??)
* forced the installer to restart/reboot servers

It was so bad - that our company support team would spend hours to try to
fix up customers - and - it just didn't work. I joined some of the calls 
as well. The biggest hint I got was: the files were always delivered to 
the wrong folder location - or even server share.

I talked to my manager - and he asked how long it would take. I said 
'one week of coding and maybe another week scripting' (there was a proprietary
scripting effort associated with this as well). 

Conversation:

* Manager: "it's going to be too expensive".
* Me: 'It's already costing the support team money to fix installations after hours and it will only get worse..'.
* Manager giving in: "What do you need?"
* Me: "I need Trent (the original product developer) for the scripting and installer part"<sup>1</sup>
* Manager: "We're going to redo the installer??"
* Me: "We need to get rid of that crazy ass hourglass"

<sup>1 I'm not 100% certain if Trent worked on the new installer but I'm pretty certain I 
talked to him about this.</sup>

The solution:
=============
The issue was that redmon/ghostscript typically runs under a system account
because (tadaa) it's a driver. A system account does not have any knowledge
of a user's account, let alone user specific directory information. The idea
was:

1. Get a user session from RedMon
2. Pass this session to the (new) software
3. Find the associated user
4. Get a pid/handle from any executable that is being run by the user (in our case this was "The Product").
5. Pass that handle on to the Windows Impersonation API
6. When logged on as user (sorry, impersonated) - get user specific information...
7. Write PDF information
8. Hand userfolder information over to the code that Trent would work on (registry I think?)
9. The script code would do the essential part by pointing "The Product" to the location of the PDF information.
9. Test with customer
10. Done and deploy to prod
11. Happy support and customers

Anyway - I have the feeling it took less than the two weeks and we did take 
out that crazy ass hourglass. Personally - The project itself was seen as one of the
best projects we worked on. It was extremely high-risk (we had sort of an idea what 
was happening but were not totally sure but my POC eventually proved what the
issue was) and we literally had to throw out old code and replace it with a new
set of executables, installer and scripts. Let alone do a combined release of all
affected and changed components...
