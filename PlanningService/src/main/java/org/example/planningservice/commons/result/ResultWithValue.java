package org.example.planningservice.commons.result;

public class ResultWithValue<T> extends Result {
    private final T value;

    private ResultWithValue(boolean isSuccess, T value, ServiceError error) {
        super(isSuccess, error);
        this.value = value;
    }

    public static <T> ResultWithValue<T> success(T value) {
        return new ResultWithValue<>(true, value, null);
    }

    public static <T> ResultWithValue<T> failureWithValue(ServiceError error) {
        return new ResultWithValue<>(false, null, error);
    }
    public T getValue() {
        return value;
    }
}
