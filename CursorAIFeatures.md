# Cursor AI Features: Rules, Skills & Subagents

## 1. Rules

**What**: Persistent instructions that shape how the AI agent behaves in your project.
They live as `.mdc` files inside `.cursor/rules/` and are automatically picked up by the agent.

**Why**: Instead of repeating "use Result pattern", "follow clean architecture", etc. in every chat,
you write it once as a rule and the agent always follows it.

**Types**:

| Type | `alwaysApply` | `globs` | When active |
|------|:---:|:---:|---|
| Always apply | `true` | — | Every conversation, always |
| File-scoped | `false` | `**/*.cs` | Only when matching files are open/referenced |

**File format** (`.mdc`):

```markdown
---
description: Brief summary shown in the rule picker
globs: **/*.cs
alwaysApply: false
---

# Rule Title

Your instructions here...
```

**Location**: `.cursor/rules/` in the project root.

---

## 2. Skills

**What**: Reusable knowledge packs that teach the agent how to perform specific tasks —
reviewing PRs, generating reports, processing files, etc.
Each skill is a directory with a `SKILL.md` file and optional reference files / scripts.

**Why**: Skills go deeper than rules. While a rule says "follow this convention",
a skill says "here's exactly how to do this workflow, step by step".

**Storage locations**:

| Type | Path | Scope |
|------|------|-------|
| Personal | `~/.cursor/skills/skill-name/` | All your projects |
| Project | `.cursor/skills/skill-name/` | Shared via repository |

**Directory structure**:

```
skill-name/
├── SKILL.md              # Required — main instructions
├── reference.md          # Optional — detailed docs
├── examples.md           # Optional — usage examples
└── scripts/              # Optional — utility scripts
```

**SKILL.md format**:

```markdown
---
name: skill-name
description: What this skill does and when to use it
---

# Skill Title

## Instructions
Step-by-step guidance...

## Examples
Concrete examples...
```

---

## 3. Subagents

**What**: Independent AI agents that can be launched to handle tasks autonomously.
They run in their own context and return results when done.

**Types**:

| Type | Description |
|------|-------------|
| `generalPurpose` | Researching, searching code, multi-step tasks |
| `best-of-n-runner` | Runs in an **isolated git worktree** on a separate branch |

**Why**: Subagents let you parallelise work. You can launch multiple agents
at the same time, each working on a different task or branch.

**Key properties**:

- Each `best-of-n-runner` gets its own branch and working directory
- Multiple agents can run in parallel without interfering with each other
- Agents can make commits on their isolated branches
- Your current branch stays untouched
- Results are returned to the main agent when the subagent finishes

**Example use cases**:

- Run two agents in parallel: one adds a feature, the other writes tests
- Try N different approaches to a refactoring and pick the best one
- One agent works on backend changes, another on frontend, each on its own branch

---

## Comparison

| Feature | Rules | Skills | Subagents |
|---------|-------|--------|-----------|
| **Purpose** | Coding conventions & standards | Complex workflows & domain knowledge | Parallel/isolated task execution |
| **Persistence** | Always available in project | Always available (personal or project) | Launched on demand |
| **Scope** | Global or file-pattern based | Triggered by task description | Scoped to a single task |
| **Complexity** | Simple (< 50 lines ideal) | Medium (< 500 lines + reference files) | N/A — they use other tools |
| **Shared via git** | Yes (`.cursor/rules/`) | Yes (`.cursor/skills/`) or personal | No |

---

## Quick Start

1. **Create a rule**: Add `.cursor/rules/my-rule.mdc` with frontmatter + instructions
2. **Create a skill**: Add `.cursor/skills/my-skill/SKILL.md` with frontmatter + workflow
3. **Launch a subagent**: Ask the AI to "run this task on a separate branch"
