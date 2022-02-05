[Ink](https://www.inklestudios.com/ink/) is a narrative scripting language. This code uses Ink stories within Unity games to control the development of a narrative.

We add a number of `Directions` that can be embedded into an Ink script, these instruct the engine to do something specific at that point. For example, we can instuct an actor to move to a specific place in the scene, change the camera angle or play a specified piece of music. For a full list of directions see below.

# Quick Start

We assume that you have a script already and want to integrate it into your game environment. 

The system supports speaker names in the form of `Speaker: Spoken words.` Although it is not required it is a good idea to have your speaker names be the same as the
actor Game Objects in the scene. This allows the Ink manager to discover those objects and control them, such as preventing the actors brain from choosing an alternative action until after a narrative section has passed.

## Unity Setup

1. Switch on the new Input System `Edit -> Project Settings -> Player -> Active Input Handling`
2. Install Cinemachine and add at least one virtual camera `GameObject => Create -> Cinemahchine -> Virtual Camera`
3. Install Text Mesh pro
4. Add an `AudioSource` component to your main camera
5. Install The [Ink Unity integration](https://github.com/inkle/ink-unity-integration).

## Scene Setup

1. Add `Ink Canvas` prefab
2. Add `Ink manager` component to a suitable manager object
3. Drag your script JSON into the `Ink JSON` field\
4. Drag your Cinemachine camera brain into the `Cinemachine` parameter
5. Drag your camera (with audio source) into the `Music Audio Source` parameter

# Directions

Directions are embedded instructions in the Ink story that tell the game engine to perform some action. For example, we can instuct an actor to move to a specific place in the scene, change the camera angle or play a specified piece of music. This section contains details of each of the directions.

Directions are inserted with the following syntax:

```
>>> DIRECTION_NAME: PARAM1, PARAM2, ...
```

Note that actors and cues that are to be used need to be setup in the `InkManager` before the game starts.

## Cue

Prompt an actor with a specific cue. Note that cues must be known to the InkManager by adding them to the Cues collection in the inspector.

```
>>> Cue: ACTOR_NAME CUE_NAME
```

## TurnToFace

The actor will turn to face the object identified.

```
>>> TurnToFace: ACTOR_NAME OBJECT_NAME
```

## PlayerControl

Sets the player control to either On or Off. If on then the Ink story will not progress and no UI will be shown until it is renabled
with a call to `SetPlayerControl(false)` in the API. Typically this would happen when a player takes a particular action in the game.

```
>>> PlayerControl: [ON_OR_OFF]
```



## SetEmotion

## Action

Tell an actor to prioritize a particular behaviour. Under normal circumstances
this behaviour will be executed as soon as possible, as long as the necessary
preconditions have been met and no higher priority item exists.

```
>>> Action: ACTOR_NAME, BEHAVIOUR_NAME
```

## AnimationParam

Set an animation parameter on an actor. 

```
>>> AnimationParam: ACTOR_NAME PARAMETER_NAME VALUE_IF_NOT_TRIGGER
```

## Audio

## Camera

Switch to a specific camera and optionally look at a named object.

```
>>> Camera: CAMERA_NAME OPTIONAL_TARGET_NAME
```

## MoveTo 

Move and actor to specific location. It is up to the ActorController to decide how they should get there.

By default the story will wait for the actor to reach their mark before continuing.
Add a 'No Wait' parameter in position 3 to allow the story to continue without waiting.

```
>>> MoveTo: ACTOR_NAME, LOCATION_Name [, Wait|No Wait]
```

### Examples of MoveTo

Move the Actor named `Pat` to a location named `Mark1`, and pause the story until they arrive.

```
>>> MoveTo: Pat, Mark1
```

Identical to above, but with an explicit wait instruction. This serves no technical purpose but can add readability.

```
>>> MoveTo: Pat, Mark1, Wait
```

Move the Actor named `Pat` to a location named `Mark1`, but allow the story to continue while they move.

```
>>> MoveTo: Pay, Mark1, NoWait
```

## Music

## StopMoving

## WaitFor

Wait for a particular game state. The actor will decide on how to get to the location.

Supported wait states are:
  * ReachedTarget - wait until an actor has finished moving to a target
  * Time - wait a specified amount of time

```
>>> WaitFor: ACTOR_NAME, LOCATION_Name [, Wait|No Wait]
```

### Examples of WaitFor command

#### ReachedTarget
Waits for the actor to have reached their current move target. 
If the target is changed during this wait cycle this wait will end when reaching the new target.

```
>>> WaitFor: ACTOR_NAME, ReachedTarget
```

#### Time
Waits for a duration (in seconds)

```
>>> WaitFor: 8
```


