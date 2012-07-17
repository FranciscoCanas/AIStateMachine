AI State Machine using probabilistic transitions.

Description:
A simple finite state machine class implemented in C# and meant for use 
in video game AI decision making. Rather than using a deterministic 
transition function for moving from state to state, this machine uses a 
probabilistic function based on Markov Chains: Each state has an 
associated vector which holds the probability of transitioning to every 
other possible state (including itself).

TODO: 
Add methods for initializing the state machine by:
a) Reading a txt/XML file detailing number of states, their names, 
and their transition vectors.
b) Receiving a data struct containing all of the above information.

