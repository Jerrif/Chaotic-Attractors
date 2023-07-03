I changed `fixed timestep` from 0.02 to 0.01 in the options. This made the trails look much better (smoother) at the cost of performance.

All the `OnBlah()` functions are from Unity's new input system. The name of the function is the name I gave the movement control, prefixed by `On`.

### About performance:
By far the largest performance bottleneck is the trails. Turning trails off increases performance from ~100 fps to >300 fps.
(500 points)
I'm not really sure why this is the case, as the GPU isn't working very hard; it's mostly the CPU.
That suggests to me that the bottleneck is actually the CPU -> GPU communication of the mesh data required to draw the 500 separate meshes.
I _think_ this would be solved with a compute shader, and doing basically (literally?) all the work straight on the GPU.
That whole process should be pretty similar to the `GPU Graph` tutorial I did on CatlikeCoding.


## Other random notes:
Funnily enough, by far the most difficult part of this whole little project was the movement controls. Specifically the mouselook. Angles and quaternions are hard, man.


## Things I could still do:
* Change trail colours
* Compute shader for trails
* Expose more options as hotkeys (attractor speed, particle size etc)
* Or do a full ESC GUI type thingo