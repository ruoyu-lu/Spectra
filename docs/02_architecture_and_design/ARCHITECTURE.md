![architecture drawio-2](https://github.com/user-attachments/assets/aa5efa01-2e32-4c15-86cd-56a6ecb3e4c7)
# Core Flow and Component Descriptions:

1.  **User & Frontend (React):**
    Users interact with the application through a **React**-based frontend running in their web browser. The frontend is responsible for rendering the user interface, collecting user input, and communicating with the backend services.

2.  **Authentication (External Services - Azure AD B2C):**
    User registration, login, and other identity management operations are handled by **Azure AD B2C**. This robust and secure external identity service issues tokens to authenticated users, which the frontend includes in subsequent API requests.

3.  **API Gateway & Edge (Azure API Management):**
    All API requests from the frontend are routed through **Azure API Management (APIM)**. APIM acts as a unified entry point, responsible for request routing, security policies (like token validation, rate limiting), API versioning, caching, and monitoring. It decouples the frontend from the backend microservices.

4.  **Backend:**
    The backend adopts a microservice architecture, comprising two key aspects:
    *   **.NET Aspire (Orchestration - Definition):** This represents the use of .NET Aspire to **define and organize** the backend microservices, their dependencies, configurations, and local development environment. Aspire simplifies the development and deployment manifest generation for cloud-native applications. It's not a direct runtime component itself but serves the development and deployment preparation phases.
    *   **Kubernetes (Runtime):** The individual backend microservices defined by .NET Aspire (such as `Image Service API`, `User Profile API`, `Feed Service API`, `Notification API`, `Auth API`) are ultimately deployed and run as containers within a **Kubernetes** cluster. Kubernetes is responsible for the actual execution, scaling, fault tolerance, and rolling updates of these services.

    **Specific backend microservices include:**
    *   `Image Service API`: Handles image uploads, downloads, and potentially metadata management (interacting with PostgreSQL).
    *   `User Profile API`: Manages user profile information.
    *   `Feed Service API`: Generates personalized content feeds for users.
    *   `Notification API`: Manages notification-related logic.
    *   `Auth API`: Potentially handles application-specific authorization logic (e.g., determining user permissions for specific actions after AD B2C authentication) or deeper integrations with AD B2C.

5.  **Data & Storage:**
    *   **Azure Blob Storage:** Used for storing user-uploaded images and other binary large objects, offering high durability and scalability. The `Image Service API` interacts with it.
    *   **PostgreSQL (e.g., Azure Database for PostgreSQL):** Stores structured application data, such as user information, image metadata (descriptions, tags), social graphs (follow lists), comments, etc. This relational database provides strong consistency and support for complex queries. Services like `User Profile API` and `Feed Service API` would interact with this database.

6.  **CI/CD (Continuous Integration/Continuous Deployment):**
    *   **GitHub (CI/CD Icon):** Serves as the code repository and likely triggers the CI/CD pipeline.
    *   **Azure Container Registry (ACR):** The CI/CD pipeline builds Docker images for the microservices and pushes them to ACR.
    *   **Deployment to Kubernetes:** The Kubernetes cluster pulls the latest images from ACR to deploy or update the services.
