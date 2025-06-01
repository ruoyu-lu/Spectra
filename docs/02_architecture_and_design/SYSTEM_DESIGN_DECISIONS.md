# System Design Decisions

This document records the key design decisions made for the Spectra project.

## 1. Backend Framework

**Decision:** Use **.NET Aspire** as the primary backend framework.

**Rationale:**

*   **Cloud-Native Focus:** .NET Aspire is designed specifically for building observable, production-ready, and distributed applications, aligning with our goal of creating a cloud-native image sharing platform.
*   **Simplified Orchestration:** Aspire provides built-in features for service discovery, resilience, and configuration, which simplifies the development and local orchestration of microservices. This is particularly beneficial given our microservice architecture outlined in `ARCHITECTURE.md`.
*   **Developer Productivity:** Leverages the extensive .NET ecosystem, C# language features, and a familiar development experience for .NET developers, potentially speeding up development.
*   **Integration with Azure:** Seamless integration with Azure services (as indicated in `REQUIREMENTS.md` EIR5.3.1 - EIR5.3.4) like Azure AD B2C, Azure Blob Storage, PostgreSQL (via Azure Database for PostgreSQL or self-hosted), and Azure Functions. Aspire facilitates the configuration and connection to these resources.
*   **Observability:** .NET Aspire applications are observable by default, making it easier to monitor, log, and trace application behavior, which is crucial for maintaining system health (MAINT5.4) and troubleshooting (REL3.4).

**Alternatives Considered:**

*   **Node.js (Express/NestJS):** While excellent for I/O-bound tasks and offering a large JavaScript ecosystem, the team's existing .NET expertise and Aspire's tailored features for .NET cloud-native development made .NET Aspire a more strategic choice.
*   **Python (Django/Flask):** Strong for rapid development and data science, but again, not as aligned with the existing .NET ecosystem focus and Aspire's specific benefits.

## 2. Frontend Framework

**Decision:** Use **React** as the frontend framework.

**Rationale:**

*   **Component-Based Architecture:** React's component model promotes reusability and modularity, leading to a more maintainable and scalable frontend codebase (MAINT5.1, MAINT5.2).
*   **Large Ecosystem and Community:** A vast collection of libraries, tools, and a strong community support makes it easier to find solutions and experienced developers.
*   **Performance:** Virtual DOM and efficient update mechanisms contribute to good rendering performance, helping meet responsiveness requirements (PERF1.1, USAB2.1).
*   **Declarative UI:** Simplifies the process of building complex user interfaces and managing application state.
*   **Compatibility with .NET Backend:** React can be effectively integrated with .NET backends, often through RESTful APIs (EIR5.2.1).

**Alternatives Considered:**

*   **Angular:** A comprehensive framework, but can have a steeper learning curve and is more opinionated than React.
*   **Vue.js:** Known for its simplicity and ease of integration, but React's larger ecosystem and job market presence were considered more advantageous for this project.
*   **Blazor WebAssembly:** While offering a full-stack .NET experience, the decision was made to leverage React for its mature frontend ecosystem and wider adoption for complex UIs.

## 3. Local Development Orchestration

**Decision:** Utilize **.NET Aspire** for local development orchestration.

**Rationale:**

*   **Unified Development Experience:** As we are using .NET Aspire for the backend, using it for local orchestration provides a consistent and streamlined development workflow. Developers can define, configure, and run all backend services and their dependencies from a single manifest.
*   **Simplified Microservice Management:** Aspire's AppHost project makes it easy to manage multiple microservices locally, including starting, stopping, and debugging them collectively. This directly supports the microservice architecture outlined in `ARCHITECTURE.MD`.
*   **Service Discovery and Dependencies:** Aspire handles local service discovery and dependency injection (e.g., connection strings for databases, URLs for other services) automatically, reducing manual configuration efforts and potential errors.
*   **Dashboard:** The Aspire dashboard provides a valuable overview of running services, logs, and traces locally, aiding in debugging and understanding inter-service communication.
*   **Alignment with Production:** While not a production runtime itself (Kubernetes is planned for that, as per `ARCHITECTURE.MD`), Aspire helps define the application structure in a way that is more easily translated to production deployment manifests.

**Why .NET Aspire was Chosen:**

*   **.NET-Native Integration:** Aspire provides seamless integration with .NET projects, automatic service discovery, and built-in support for .NET development patterns.
*   **Simplified Development Experience:** The integrated dashboard, automatic container orchestration, and hot reload capabilities significantly improve the developer experience.
*   **Production Alignment:** While Aspire is primarily for local development, it helps structure applications in a way that translates well to production Kubernetes deployments.
*   **Reduced Complexity:** Eliminates the need for separate Docker Compose configurations and provides a unified development experience within the .NET ecosystem.

## 4. Key Architectural Patterns & Decisions

*   **Microservices Architecture:** As detailed in `ARCHITECTURE.md`, the backend will be composed of several independent microservices (e.g., Image Service, User Profile Service).
    *   **Reason:** Enhances scalability (SCAL6.1), maintainability (MAINT5.2), and allows for independent deployment and technology choices (if needed in the future) for different services.
*   **API Gateway (Azure API Management):** As per `ARCHITECTURE.md`.
    *   **Reason:** Centralizes cross-cutting concerns like authentication, rate limiting, routing, and provides a stable interface for the frontend (EIR5.2.1).
*   **Externalized Identity Management (Azure AD B2C):** As per `REQUIREMENTS.md` (EIR5.3.1) and `ARCHITECTURE.md`.
    *   **Reason:** Leverages a robust, secure, and scalable identity solution, offloading complex identity management from the application.
*   **Asynchronous Operations (Azure Functions):** For tasks like image post-processing or notifications, as mentioned in `REQUIREMENTS.md` (EIR5.3.4).
    *   **Reason:** Improves application responsiveness by offloading long-running or non-critical tasks to background processes (PERF1.2, PERF1.3).
*   **Cloud Storage for Images (Azure Blob Storage):** As per `REQUIREMENTS.md` (EIR5.3.2) and `ARCHITECTURE.md`.
    *   **Reason:** Provides a durable, scalable, and cost-effective solution for storing large binary files (IR2.1).
*   **Database Choice (PostgreSQL):** The primary database system will be PostgreSQL. This can be hosted on Azure (e.g., Azure Database for PostgreSQL) or self-managed.
    *   **Reason:** PostgreSQL is a powerful, open-source object-relational database system with a strong reputation for reliability, feature robustness, and performance. It offers advanced SQL features, extensibility, and strong community support. It can effectively handle relational data for user profiles, image metadata, social graphs, and comments (SCAL6.2). Its ACID compliance is crucial for transactional integrity.

## 5. Future Considerations / To Be Decided (TBD)

*   **Specific PostgreSQL Deployment Strategy:** Decision on whether to use a managed service like Azure Database for PostgreSQL or a self-hosted instance (e.g., in Kubernetes).
*   **Caching Strategy:** Detailed caching mechanisms (e.g., CDN for images, Redis for API responses or session data) to further improve performance (PERF1.1).
*   **Detailed CI/CD Pipeline Implementation:** Specific tools and configurations for the CI/CD pipeline (e.g., GitHub Actions workflows, Azure DevOps Pipelines).
*   **Monitoring and Alerting Details:** Specific metrics to monitor and alerting thresholds using Azure Monitor (MONITORING_AND_LOGGING.md).

This document will be updated as the project evolves and further design decisions are made. 