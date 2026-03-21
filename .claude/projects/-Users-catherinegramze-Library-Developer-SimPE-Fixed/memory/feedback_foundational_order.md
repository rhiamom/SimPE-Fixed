---
name: foundational_order
description: Do foundational/dependency work before dependent files — never stub a foundation to unblock a dependent
type: feedback
---

Do the foundational layer first, then the files that depend on it.

**Why:** Porting dependent files before their foundations forces stubs everywhere. Stubs accumulate and must all be undone later. The user explicitly wants real Avalonia implementations, not stub shims.

**How to apply:** When a dependent project needs a library (e.g. NetDocks, Avalonia docking), port/replace that library first. Only then fix the files that reference it. Never create a stub to unblock a dependent — fix the dependency properly instead.
