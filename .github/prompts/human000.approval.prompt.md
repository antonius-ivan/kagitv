---
description: Human000 approval gate for Aivan. Use for go/no-go decisions after speckit.specify, after speckit.plan, before major implementation, and after QA final review.
---

## User Input

```text
$ARGUMENTS
```

Use this prompt when a human approval checkpoint is required.

## Procedure

1. Identify the approval stage from the input or current context:
   - Specification approval
   - Plan approval
   - Implementation start approval
   - Final ship-or-iterate approval
   - Phase closure follow-up when QA has validated a completed phase
2. Summarize the artifact under review in plain language:
   - purpose
   - scope
   - major risks
   - open questions
   - recommended decision
3. Present the approval request to `Human000` using this exact structure:

### Human000 Approval Request

**Stage**: [specify | plan | implement | release]
**Artifact**: [path or item under review]
**Summary**: [2-5 concise lines]
**Risks**: [flat list of key risks or `None`]
**Recommendation**: [approve | approve with conditions | revise]

If `Stage` is `release` and QA has already validated a completed phase, include whether the phase file list artifact has been updated or still requires Human000 action.

**Human000 Decision**:
- `Approve`
- `Approve with conditions: ...`
- `Revise: ...`

4. Do not assume approval. Wait for the human decision.
5. If conditions are added, restate them as binding next-step constraints.
6. If the decision is `Revise`, identify which role should respond next: `TPM001`, `DBA002`, `CsharpBackend003`, `TSFrontend004`, or `QAA005`.
