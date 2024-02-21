
Footstepper: Complete Footstep Solution
https://gamingislove.com/products/footstepper/

Need support? Drop by the support forum:
https://forum.orkframework.com/

Or contact me via email:
contact@gamingislove.com


-------------------------------------------------------------------------------------------------------------------------------------------------------
Quick Setup Guide
-------------------------------------------------------------------------------------------------------------------------------------------------------

Create reusable 'Footstep Materials' assets to set up the audio clips and prefabs (e.g. for particle effects) 
that should be used for something - you can set up separate clips/prefabs for walking, running, sprinting, 
jumping and landing.
They're created in the Project tab in Unity.

Create reusable 'Footstep Texture Materials' assets to link textures and sprites to footstep materials - this is used 
by terrains to determine the ground you're moving on and can also be used to determine the ground based on the 
renderer of a game object.
They're created in the Project tab in Unity.

Add footstep sources to game objects in your scenes:
- 'Object Footstep Sources' add footstep materials to individual game objects 
(e.g. for stones, wooden planks, brides or water).
- 'Terrain Footstep Sources' handle finding the correct footstep material for terrains
- 'Tilemap Footstep Sources' handle finding the correct footstep material for tiles of a tilemap
- 'Trigger Footstep Sources' add footstep materials to an area in your scene and overrule all other sources 
(e.g. for water or to make sure things in an area sound the way you want).

Add a 'Footstepper' component to something that should use footsteps (e.g. the player).

Set up how footsteps are played.

More detailed explanations can be found in the documentation.


-------------------------------------------------------------------------------------------------------------------------------------------------------
Documentation
-------------------------------------------------------------------------------------------------------------------------------------------------------

You can find the full documentation here:
https://gamingislove.com/products/footstepper/#documentation

Or the individual parts here:

1. First Steps:
https://gamingislove.com/tutorials/footstepper/first-steps/

2. Footstep Materials:
https://gamingislove.com/tutorials/footstepper/footstep-materials/

3. Footstep Sources:
https://gamingislove.com/tutorials/footstepper/footstep-sources/

4. Footsteppers:
https://gamingislove.com/tutorials/footstepper/footsteppers/

5. Playing Footsteps:
https://gamingislove.com/tutorials/footstepper/playing-footsteps/

6. Footstep Manager:
https://gamingislove.com/tutorials/footstepper/footstep-manager/

7. Foot IK:
https://gamingislove.com/tutorials/footstepper/foot-ik/

8. Editor Tips:
https://gamingislove.com/tutorials/footstepper/editor-tips/

9. Effect Tags:
https://gamingislove.com/tutorials/footstepper/effect-tags/

Integrations:
https://gamingislove.com/tutorials/footstepper/integrations/


-------------------------------------------------------------------------------------------------------------------------------------------------------
Demo Assets
-------------------------------------------------------------------------------------------------------------------------------------------------------

Download the demo assets here:
https://gamingislove.com/products/footstepper/#demo-assets

The 'Footstepper Demo' includes a 2D and 3D demo scene.
The demo uses assets from Unity's 'Standard Assets'
(https://assetstore.unity.com/packages/essentials/asset-packs/standard-assets-for-unity-2017-3-32351)
as well as audio clips and textures from opengameart.org (https://opengameart.org/).


To get the 'Footstepper Demo' running, do the following:
- import the unitypackage into a Unity project (matching the version of the demo)
- import Footstepper into the Unity project
- open the build settings: 'File > Build Settings...' in the Unity menu)
- add the 2 demo scenes to the 'Scenes In Build':
	- Footstepper/Demo/Demo3D/Footstepper Demo 3D
	- Footstepper/Demo/Demo2D/Footstepper Demo 2D


-------------------------------------------------------------------------------------------------------------------------------------------------------
Version Changelog
-------------------------------------------------------------------------------------------------------------------------------------------------------

Version 1.8.0:
- new: Mesh Footstep Source: Plays footstep effects based on the materials of the mesh renderer at the raycast hit's position (mesh must be readable, i.e. 'Read/Write Enabled' in it's import settings). Uses footstep texture materials to find footsteps based on renderer materials. If no material is found, uses the footstep manager as fallback.
- new: Footstep Texture Materials: 'Materials' settings available. Link footstep materials to materials used by renderers. This is used by 'Mesh' footstep sources or when stepping on something with a 'Mesh Renderer' (and using renderer fallbacks). If no matching material is found, it'll try to find a footstep effect based on the material's main texture.
- new: Footstep Texture Materials: 'UV Data' settings available. Link footstep materials to textures and (renderer) materials based on UV texture coordinates. This allows using atlas textures/materials for footsteps. Textures/materials are bound to min and max UV texture coordinates, e.g. X=0,Y=0 to X=0.5,Y=0.5 for the first texture (UV coordinates originate in the lower left corner of the texture).
- new: Footstep Materials: Prefabs: 'Index Prefabs' settings available. Optionally add different prefabs for a defind foot index, e.g. to spawn separate prefabs for left and right foot.
- new: Footstepper: 'Search Renderers' now also supports finding material based footstep effects from a footstep manager's footstep texture materiels. Tries to find the material based on the hit position on the mesh.
- new: Footstep Trigger: 'Foot Index' setting available. Define the index of the foot the footstep trigger is used for. This is used by index prefabs of footstep materials to find the foot's matching prefab.
- fix: Footstepper: Added the missing 'Top Down 2D Mode' setting that was removed by mistake in 1.7.0.


Version 1.7.0:
- change: Terrain Footstep Source: No longer caches the whole alphamap of the terrain on initialization, instead getting the small part at the checked step position.
- fix: Footstep Effects: Fixed an issue where playing a random effect never played the last in the list.
- fix: Footstepper, Trigger Footstep Source: Fixed an issue where disabling and enabling a collider (on a game object with a 'Footstepper') while being within a 'Trigger Footstep Source' could cause for the footstep source to keep being used after leaving it.


Version 1.6.0:
- new: Footstepper: 'Top Down 2D Mode' available in 'Speed Settings'. Uses the X and Y axes to calculate movement speed instead of X and Z axes.
- change: Tilemap Footstep Source: Now checks sprites on other tilemaps when no footstep effect is found for a tile's sprite (when multiple tilemaps are used).


Version 1.5.0:
- new: Footstep Manager: 'Player' and 'Allowed Distance To Player' settings available. Optionally only allow 'Footstepper' components within a defined distance to a player 'Footstepper' to play footsteps. Can be set via code to change the used player.
- new: Footstepper: 'Is Player' setting available. Automatically sets the 'Foostepper' as the 'Footstep Manager' player for allowed distance checks.
- new: Footstepper: 'On Footstep' event available. Register your (parameterless) function with a 'Footstepper' component, the function will get called whenever a footstep is played.
- new: Footstepper: 'On Footstep Detailed' event available. Like 'On Footstep', but with more detailed information. Passes on the foot's 'Transform', the used 'FootstepEffect', the 'Vector3' position and 'Vector3 normal that where hit by the raycast.


Version 1.4.0:
- new: Footstep Trigger: 'Limit Layer' settings available. Optionally limit the layers that can cause footsteps.
- new: Footstepper: 'Speed Update Type' setting available. Select when the 'Footstepper' component's current movement speed is updated, either 'None' (no automatic calculation), 'Update', 'Late Update' or 'Fixed Update'. This should match how the game object is moved (e.g. 'Fixed Update' when using rigidbodies for movement).
- new: Scripting: Footstepper: You can now set the speed of a 'Footstepper' component (e.g. used for speed checks or auto play) via the 'Speed' property (Vector2).


Version 1.3.0:
- new: Footstep Materials: 'Custom Effects' settings available in 'Default Effects' and 'Tag Effects'. Custom footstep effects are identified by their 'Custom Name' and can be played using the 'FootstepCustom' function name in animation events (providing 'Int' value for foot index and 'String' value for custom effect name) and 'FootstepCustomIndex' function via scripting. Use it to play other effects, e.g. sliding.
- new: Footstepper: 'Custom Effect Volume' setting available. Defines the volume used to play custom footstep effects.


Version 1.2.0:
- new: Footstepper: 'Mode' setting available. Defines if the footstepper is enabled, disabled or only plays audio clips or spawns prefabs.
- new: Footstepper: 'No Raycast Fallback' setting available. Optionally use the fallback material even if the raycast didn't hit anything.


Version 1.1.0:
- new: Tilemap Footstep Source: Plays footstep effects based on the sprite of the tile at the raycast hit's position. You can use multiple tilemaps in a single source, the first tilemap that has a sprite for the position will be used. Requires Unity 2017.2 or newer.
- new: Footstepper: 'Auto Find' settings available. If no footstep source was found on the game object hit by the raycast, the footstepper can search for effects based on hit tilemaps or renderers. This is now optional.
- new: Gizmo Icons: Added new gizmo icons for 'Footstepper', 'Footstep Manager', 'Foot IK', 'Footstep Trigger', 'Object Footstep Source', 'Terrain Footstep Source', 'Tilemap Footstep Source' and 'Trigger Footstep Source' components.
- new: Footstep Materials, Footstep Texture Materials: Materials now have their own, separate icons.


Version 1.0.0:
- Initial Release