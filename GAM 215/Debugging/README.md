# Debugging (GAM 215, Module 5, Assignment 5.1: Debugging)

## Main file locations:

* Location of script files (`Assets/Debug Exercises/Scripts/`): `Debug1.cs`, `Debug2.cs`, and `Debug3.cs`
* Location of scene files (`Assets/Debug Exercises/Scenes/`): `Debug1.unity`, `Debug2.unity`, and `Debug3.unity`

## Project information

I have opened all the debug scripts and fixed the syntax errors, runtime errors, and logic errors.  I have also added
comments as required.

### Unity Scene

Did not add anything to the scene.

### Script Info

1. In `Debug1.cs`, I did the following:
  1. Fixed the syntax errors for declaring `controller` and `velocity` by specifying the `CharacterController` type
  for `controller` and adding `velocity` as the variable name for the incomplete `private Vector3` statement
  2. Added missing variable for `speed`
  3. Added return type for `Start()`
  4. Added closing brace `}` for `Start()`
  4. Initialized `controller` variable
  5. Replace incorrect braces `{}` with parentheses `()` for the `Update()` function
  6. Change `if(Input.GetKey("w") = true` to use comparison `==` instead of assignment `=` and add closing parenthesis `)`
  7. For `velocity z = speed`, add missing dot `.` operator for `velocity z` and insert missing semicolon `;` at the end of the statement
  8. Change the assignment operators `=` in `controller.Move (transform.forward = velocity.z = Time.deltaTime);` to be multiplication `*`
  (this is both a syntax error and a logic error)
  9. Add missing brace `}` at the end of `void Update()`
2. In `Debug2.cs`, I did the following:
  1. Initialized `controller` variable
  2. Zero out the `x` and `z` values for `velocity` (Zeroing out `x` was probably unnecessary, but I figured there is no harm in doing so; especially since we're not strafing)
  3. Fix `guiObject.guiText.text = transform.position.ToString();` to be `guiObject.GetComponent<GUIText>().text = transform.position.ToString();`
  (This may not be an error in the old example, but the current Unity version has deprecated the old method of accessing components)
3. In `Debug3.cs`, I did the following:
  1. Zeroed out the `x` and `z` values for `velocity` (This step was missing in the example)
  2. Changed `velocity.z -= -speed;` to `velocity.z = -speed;` (This double negative makes the character move forward instead of backward)
  3. Swap the `Rotate()` calls for "look left" and "look right", since they were moving in the opposite directions
  4. Changed `velocity.y = 5;` to `velocity.y = jumpSpeed;`, since we want to utilize the variable we have set
  5. Changed `velocity.y += 3 * Time.deltaTime;` to `velocity.y += Physics.gravity * Time.deltaTime;` (Not sure if it was necessary, but I wanted to use gravity's value)
  6. Changed `transform.up * velocity.z * Time.deltaTime` in the `controller.Move()` call to be `transform.up * velocity.y * Time.deltaTime`
  (We want to affect the Y velocity on `transform.up`, not Z)
  7. Fix `guiObject.guiText.text = transform.position.ToString();` to be `guiObject.GetComponent<GUIText>().text = transform.position.ToString();`
4. I also replaced `public` variables with `[SerializeField] private` to match assignment 4's recommended variable usage
5. I also added `private` for the `void Start()` and `void Update()` functions -- I just like being a little more explicit in my scripts

Author: Jv Chen

## How does my project satisfy the conditions of the assignment?

1. I have fixed all the syntax, runtime, and logic errors in the sample Unity project

## Miscellaneous

This README is a markdown file that can be generated into an HTML page at http://daringfireball.net/projects/markdown/dingus or
in my Github page (https://github.com/bluejv7/AAU-Projects/tree/master/GAM%20215/Debugging).  You can always check my Github account to see if anyone
plagiarized my work or if I have done the same to others.  Academic integrity is important!