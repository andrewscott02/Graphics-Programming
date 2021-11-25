Controls:
WASD - Movement
Left Mouse Button/ Left Ctrl -  Spin sword

Features (All scripts, shaders, materials and prefabs created for this assignment are located in the VFX folder):

-Most important scripts are the GeneratePlaneMesh and SwordTrail scripts, the others are either old scripts or tests that are no longer used

-Procedural Mesh Generation:
--Liquid Effects (Water, lava and mud), which implement mesh generation through generating waves. A collision mesh is also located below the water surface, which mimics liquid physics. The height of the vertices will also determine the colour of the waves i.e. water will appear more white at the heighest parts of the wave to appear more reflective.

--Sword trail effects when spinning sword. Utilizes a quadratic lerp function, similar to worksheet 1, to create a curve along the path upon which the trail mesh is generated.

-Shaders:
--Liquid shaders:
---Edge glow (only works in scene view) composed of intersection glow and fresnel subgraphs
---Foam effect, which makes a frothy effect on the surface and adds sparkles via the specs subgraph
---Vertex height colour, which alters the colour of the surface based on the vertex height. There is a property that determines how much this affects the material

--Sword Trail shader:
---Foam effect, which makes uneven colouring and specs on the trail
---Tiling effect, which alters the opacity of the trail

--Crystal Barrier shader (Glow):
---Pulse effect
---Fresnel effect