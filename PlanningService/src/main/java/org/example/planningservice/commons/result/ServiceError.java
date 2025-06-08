package org.example.planningservice.commons.result;

import lombok.Getter;

import java.util.Map;

@Getter
public class ServiceError {
    public static final String NOT_FOUND = "NotFoundError";
    public static final String UNHANDLED = "InternalError";
    public static final String VALIDATION = "ValidationError";
    public static final String EXISTED = "EntityExistedError";
    public static final String UNAUTHORIZED = "InvalidPermissionError";

    public static final ServiceError NONE = new ServiceError("");

    private final String code;
    private final String description;
    private final Map<String, Object> information;

    public ServiceError(String code) {
        this(code, null, null);
    }

    public ServiceError(String code, String description) {
        this(code, description, null);
    }

    public ServiceError(String code, String description, Map<String, Object> information) {
        this.code = code;
        this.description = description;
        this.information = information;
    }

    public static ServiceError notFoundError(String description) {
        return new ServiceError(NOT_FOUND, description);
    }

    public static ServiceError unhandledException(String description) {
        return new ServiceError(UNHANDLED, description);
    }

    public static ServiceError validationFailed(String description) {
        return new ServiceError(VALIDATION, description);
    }

    public String getCode() {
        return code;
    }

    public Map<String, Object> getInformation() {
        return information;
    }

    public String getDescription() {
        return description;
    }

    public static ServiceError existedError(String description) {
        return new ServiceError(EXISTED, description);
    }

    public static ServiceError unauthorizedError(String description) {
        return new ServiceError(UNAUTHORIZED, description);
    }
}
