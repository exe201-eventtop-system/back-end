package org.example.planningservice.commons.result;

public class Result {
    private final boolean isSuccess;
    private final ServiceError error;

    protected Result(boolean isSuccess, ServiceError error) {
        this.isSuccess = isSuccess;
        this.error = error;
    }

    public static Result success() {
        return new Result(true, null);
    }

    public static Result failure(ServiceError error) {
        return new Result(false, error);
    }

    public boolean isSuccess() {
        return isSuccess;
    }

    public boolean isFailure() {
        return !isSuccess;
    }

    public ServiceError getError() {
        return error;
    }
}
