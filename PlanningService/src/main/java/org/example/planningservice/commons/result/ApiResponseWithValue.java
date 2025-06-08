package org.example.planningservice.commons.result;

public class ApiResponseWithValue<T> extends ApiResponse {
    private T data;

    public ApiResponseWithValue() {
    }

    public ApiResponseWithValue(boolean success, String message, T data, String errorCode) {
        super(success, message, data, errorCode);
        this.data = data;
    }

    @Override
    public T getData() {
        return data;
    }


}
