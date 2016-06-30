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
2. 

Author: Jv Chen

## How does my project satisfy the conditions of the assignment?

1. Example of something that satisfies a condition of the assignment

## Miscellaneous

This README is a markdown file that can be generated into an HTML page at http://daringfireball.net/projects/markdown/dingus or
in my Github page (`link_to_project_folder`).  You can always check my Github account to see if anyone
plagiarized my work or if I have done the same to others.  Academic integrity is important!