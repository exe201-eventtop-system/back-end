# Stage 1: Build with Maven
FROM maven:3.9.4-eclipse-temurin-17 AS builder

WORKDIR /app

# Copy source code
COPY . .

# Build project and skip tests
RUN mvn clean package -DskipTests

# Stage 2: Run the app using JDK only
FROM eclipse-temurin:17-jdk-alpine

WORKDIR /app

# Copy JAR file from builder stage
COPY --from=builder /app/target/*.jar app.jar

# Expose port (thường là 8082 hoặc theo config)
EXPOSE 8082

# Run Spring Boot app
ENTRYPOINT ["java", "-jar", "app.jar"]
