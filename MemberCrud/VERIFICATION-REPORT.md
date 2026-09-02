# Verification Report

## Introduction

For this project, I used GitHub Copilot to review my MemberCrud application. The goal was not only to use AI, but also to check if the AI recommendations were correct.

I used Microsoft C# Coding Conventions and SOLID principles as my code review standards.

Copilot found 15 issues in my project. I reviewed the findings and decided to Accept, Reject, or Defer each one.

## Where AI Was Wrong

One important problem happened when Copilot helped me move the database connection string.

The connection string was hard-coded inside MemberCrudDbContext.cs. Copilot recommended moving it to appsettings.json.

The first solution from Copilot was not completely correct. Copilot moved the connection string to appsettings.json, but it also kept the old hard-coded connection string as a fallback.

I noticed that this did not completely fix the original problem. I asked Copilot to remove the hard-coded fallback.

I also checked the JSON connection string because the backslash for MSSQLLocalDB needed correct JSON escaping.

After my verification, Copilot corrected the solution.

This showed me that I cannot automatically accept AI-generated code. I need to read and understand the changes before using them.

## Dependency Injection Verification

Copilot also recommended using IMemberService and Dependency Injection.

The first plan still created a new MemberService inside the WinForms forms. I did not accept this plan because the forms would still depend directly on MemberService.

I asked Copilot to improve the plan.

The final solution creates MemberService in Program.cs and passes IMemberService through ChurchManagement, MemberManagement, AddMember, and EditMember.

This solution was better because the forms no longer create MemberService directly.

## Testing and EF Core Problem

After the Dependency Injection changes, the project built successfully, but some unit tests failed.

I investigated the problem with Copilot.

The main MemberCrud project used EF Core 10.0.10, but the unit test project used Microsoft.EntityFrameworkCore.InMemory 8.0.0.

The different versions caused a runtime error.

I updated Microsoft.EntityFrameworkCore.InMemory to version 10.0.10 so the versions matched.

After this correction, the available unit tests passed.

## MemberService Validation

Another accepted finding was missing argument validation in MemberService.

I used Copilot to add ArgumentNullException checks to AddMember, UpdateMember, and DeleteMember.

I also added three unit tests to verify the new behavior.

The final test result was:

- 25 tests executed
- 10 passed
- 0 failed
- 15 skipped

The skipped tests were integration-style tests that were already skipped and were not changed by this remediation.

## Recommendations I Did Not Accept

I did not accept every recommendation from Copilot.

For example, Copilot recommended additional changes for the DateOnly conversion in EditMember. The current conversion was working correctly for my application, so I rejected this recommendation because there was no demonstrated problem that required the extra change.

I also deferred some recommendations that required larger changes and were not necessary for this assignment.

## What I Used From My Previous Knowledge

My knowledge of C#, Entity Framework, unit testing, databases, and the structure of my MemberCrud project helped me verify Copilot's answers.

I knew that changing code can affect other parts of the application. Because of this, I checked the build and tests after important changes.

I also checked that ChurchManagement remained the startup form because I did not want the Dependency Injection change to change the normal application flow.

## What I Accepted Without Full Verification

I tried to verify the important changes by reviewing the code, building the solution, and running the available tests.

Some tests were skipped because they require LocalDB or integration testing. Because of this, not every possible application behavior was automatically tested.

For the important remediations, I reviewed the generated code before accepting it.

## Conclusion

This assignment taught me that AI can help find problems and suggest solutions, but AI can also make mistakes.

GitHub Copilot helped me improve my project, but I had to review its recommendations, reject some suggestions, ask for corrections, and verify the final code.

The most important lesson for me is that AI can help a programmer, but the programmer is still responsible for the final code.