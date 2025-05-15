# 🧪 Mock API Services

This directory contains mock API services for testing purposes. These services simulate the behavior of real APIs, allowing for isolated testing and development without affecting production systems.

## 📁 Structure
- **Employees/** — Mock API for employee-related services
- **Medicines/** — Mock API for medicine-related services

## 🚀 Usage
These mock services are intended for development and testing environments. They can be used to simulate API responses and test integration points without connecting to actual backend services.

## 🛠️ How They Are Built
The mock APIs are built using `json-server`, a simple and lightweight tool that allows you to create a full fake REST API with zero coding in less than 30 seconds. Each mock API is configured to serve data from a `db.json` file located in the `data/` directory.

## 🏃‍♂️ How to Run
1. Navigate to the specific mock API directory (e.g., `Employees/` or `Medicines/`).
2. Install dependencies if not already done:
   ```bash
   npm install
   ```
3. Start the mock API server:
   ```bash
   npm start
   ```
4. The mock API will be available at the specified port (e.g., `http://localhost:9002` for Employees, `http://localhost:9001` for Medicines).