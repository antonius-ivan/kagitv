# Feature Specification: Phase 001 Aspire Bootstrap

**Feature Branch**: `001-aspire-bootstrap`  
**Created**: 2026-04-02  
**Status**: Draft  
**Input**: User description: "Phase 001 is: bootstrap from the Aspire JavaScript frontend sample; normalize the repo into the Helene/Gilgamesh shape; establish src/ICLAco.AppHost, src/ICLAco.ServiceDefaults, and src/BlazeNexJ; make the Node frontend run under Aspire through AppHost; leave the repo ready for later commerce and cybersecurity phases"

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Launch Foundation Runtime (Priority: P1)

As a developer starting Glaive, I need the repository to boot through an AppHost-centered Aspire entry point so I can run the platform locally with the Node frontend managed as a first-class application resource.

**Why this priority**: No later commerce or cybersecurity feature work can proceed safely until the runtime host, solution structure, and frontend orchestration are stable.

**Independent Test**: Can be fully tested by launching the AppHost from a clean checkout and confirming that the frontend starts under Aspire, the health endpoint responds, and the resource graph is visible without manual process stitching.

**Acceptance Scenarios**:

1. **Given** a clean repository checkout, **When** a developer starts the AppHost, **Then** the Node frontend is launched by Aspire and exposed through the configured frontend endpoint.
2. **Given** the frontend is running under AppHost, **When** a developer opens the health endpoint, **Then** the endpoint confirms the frontend is alive and reachable.

---

### User Story 2 - Work In Standard Project Shape (Priority: P2)

As a developer familiar with Helene and Gilgamesh, I need Glaive to use the same `src/`-centered project layout with AppHost and shared defaults so future services and frontends can be added without restructuring the repository.

**Why this priority**: The repo shape determines how future phases will be planned, implemented, and validated. Getting it wrong in Phase 001 creates avoidable migration work later.

**Independent Test**: Can be tested independently by inspecting the generated solution and repository layout and confirming that the expected Phase 001 projects exist in their long-term locations.

**Acceptance Scenarios**:

1. **Given** the Phase 001 setup is complete, **When** a developer reviews the repository tree and solution membership, **Then** `src/ICLAco.AppHost`, `src/ICLAco.ServiceDefaults`, and `src/BlazeNexJ` exist and are represented in the solution.
2. **Given** a later backend or frontend phase is planned, **When** the plan references new projects, **Then** it can extend the existing structure without moving or renaming the Phase 001 foundation projects.

---

### User Story 3 - Prepare For Future Commerce And Cybersecurity Work (Priority: P3)

As a technical lead, I need the bootstrap phase to include baseline telemetry, health signaling, and solution conventions so later commerce services and cybersecurity dashboard features can be added on a diagnosable and governed platform.

**Why this priority**: The project is intended to evolve into a distributed ecommerce platform with a cybersecurity dashboard. The bootstrap phase must avoid creating a dead-end sample that lacks observability or extensibility.

**Independent Test**: Can be tested independently by verifying that the frontend and host expose baseline diagnostics and that the shared defaults project is positioned to be reused by future services.

**Acceptance Scenarios**:

1. **Given** the AppHost and frontend are running, **When** the developer inspects the runtime diagnostics, **Then** the platform exposes baseline health and telemetry signals suitable for local troubleshooting.
2. **Given** a later API or worker project is added, **When** it adopts the shared defaults conventions, **Then** it can align with the same operational and governance standards established in Phase 001.

### Edge Cases

- If the frontend process fails to start under AppHost, the developer must receive a clear startup failure signal rather than a silent partial launch.
- If the configured frontend port is unavailable, the phase must fail in a way that makes the conflict diagnosable.
- If required Node dependencies have not been restored, the startup flow must surface the missing dependency state clearly.
- If the repository can launch the frontend only through manual commands outside AppHost, Phase 001 is incomplete.
- If future phases add backend services, the Phase 001 structure must not require moving the frontend, AppHost, or shared defaults projects.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: The system MUST establish a repository structure rooted in `src/` that includes `ICLAco.AppHost`, `ICLAco.ServiceDefaults`, and `BlazeNexJ` in their intended long-term locations.
- **FR-002**: The system MUST include an AppHost entry point that starts and manages the Node frontend as an Aspire application resource.
- **FR-003**: Developers MUST be able to launch the Phase 001 solution through the AppHost without manually starting the frontend in a separate unmanaged workflow.
- **FR-004**: The Phase 001 setup MUST include a shared defaults project intended for reuse by later backend services for common operational conventions.
- **FR-005**: The frontend MUST expose a health-checkable runtime endpoint so developers can verify local readiness through the hosted application.
- **FR-006**: The Phase 001 solution MUST be represented in the repository's primary solution file so the foundational projects can be opened and evolved together.
- **FR-007**: The setup MUST preserve compatibility with later commerce and cybersecurity phases by avoiding sample-specific folder names or temporary structures that would require repository reorganization.
- **FR-008**: The Phase 001 foundation MUST define baseline telemetry and diagnostics expectations for the hosted frontend and AppHost lifecycle.
- **FR-009**: The developer startup experience MUST document or expose the expected success, failure, and recovery states for launching the platform locally.
- **FR-010**: The bootstrap phase MUST leave clear extension points for future APIs, workers, shared contracts, and additional frontend surfaces without changing the Phase 001 project identities.

### Key Entities *(include if feature involves data)*

- **AppHost**: The orchestration entry point that manages application resources, startup workflow, and local runtime composition.
- **Service Defaults**: The shared operational conventions package that centralizes default diagnostics, health, and platform behaviors for future services.
- **Frontend Application**: The Node-based user interface project hosted under Aspire and intended to evolve into dashboard and commerce-facing experiences.
- **Solution Workspace**: The canonical project grouping that defines the Phase 001 foundation and the paths future phases will extend.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: A developer can start the platform from the AppHost on a clean machine setup and reach the hosted frontend within 5 minutes of beginning the local run procedure.
- **SC-002**: In 100% of normal local startup attempts with prerequisites installed, the AppHost launches the frontend without requiring a second manually managed terminal process for the frontend runtime.
- **SC-003**: Developers can confirm platform readiness through a health endpoint and runtime diagnostics within 1 minute of launch completion.
- **SC-004**: The repository structure created in Phase 001 can accept at least one future backend service and one future additional frontend surface without moving the AppHost, shared defaults, or primary frontend project.

## Assumptions

- Developers performing Phase 001 work have the local prerequisites needed to run .NET Aspire and a Node-based frontend.
- Phase 001 is limited to foundation setup and does not include delivery of commerce business workflows or cybersecurity operator workflows.
- The Node frontend established in Phase 001 is the initial hosted UI surface and later phases may add more backend services and optional additional frontends.
- The project naming convention remains `ICLAco.*` for the foundational host and shared platform projects during this phase.
- Later phases will define concrete APIs, event contracts, persistence choices, and role models; Phase 001 only needs to preserve room for them.
