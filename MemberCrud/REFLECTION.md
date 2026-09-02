# Reflection

## My Experience Using AI

For this assignment, I used GitHub Copilot to review and improve my MemberCrud project.

Before this assignment, I used AI mostly when I had an error or when I needed help understanding code. This assignment showed me another way to use AI. AI can also work like a code reviewer and help me find problems in code that is already working.

Copilot found 15 findings in my project. Some findings were about code style, but others were more important, such as the hard-coded connection string, dependency injection, and missing argument validation.

## What I Can Delegate to AI

I think AI is very useful for reviewing code, finding repeated code, finding naming problems, and suggesting better programming practices.

AI can also help me understand SOLID principles and show me different ways to organize my classes.

For example, Copilot helped me create IMemberService and use Dependency Injection. It also helped me identify the EF Core version problem in my unit tests.

These are tasks where AI can save me time.

## What I Should Do Myself

I learned that I should not let AI make every decision for me.

I need to understand my project and verify the AI recommendations before accepting them.

For example, Copilot first recommended moving the connection string to appsettings.json, but it kept the old hard-coded connection string as a fallback. I noticed that the original problem was not completely fixed.

Copilot also first suggested creating MemberService inside the forms while trying to implement Dependency Injection. I did not accept that solution because the forms would still depend directly on the concrete MemberService.

I asked Copilot to correct these solutions before I accepted them.

## Time Generating vs. Time Verifying

AI generated recommendations very quickly. However, I spent more time reading, testing, and verifying the recommendations.

I had to check the code, build the project, run unit tests, and sometimes ask Copilot to change its first solution.

This showed me that generating code is fast, but verification is very important.

## What Could Happen If I Used the First AI Answer

If I shipped the first AI-generated solution without checking it, my project could have problems.

For example, the connection string would still exist inside the source code as a fallback. The first Dependency Injection plan would also still create MemberService directly inside the forms.

The code might look improved, but the original design problems would not be completely fixed.

This is why I think a programmer must always review AI-generated code.

## A New Skill I Learned

One important skill I learned from this assignment is Dependency Injection.

Before this review, my forms depended directly on MemberService. Now I understand why using an interface like IMemberService can reduce coupling between classes.

I also learned more about checking package versions. My unit tests had Microsoft.EntityFrameworkCore.InMemory 8.0.0 while the main project used EF Core 10.0.10. After matching the versions, the tests worked correctly.

At the end, 25 tests were executed, 10 passed, 0 failed, and 15 were skipped.

## Conclusion

This assignment changed the way I think about AI in programming.

AI is a helpful tool, but it is not always correct. I should use AI to help me, not to replace my own thinking.

My responsibility as a programmer is to understand the recommendation, check the code, test the application, and make the final decision.

The biggest lesson I learned is simple: AI can write code quickly, but I am responsible for verifying that the code is correct.