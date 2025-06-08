package org.example.planningservice.commons.helper;

import org.example.planningservice.commons.result.ApiResponse;
import org.example.planningservice.commons.result.ResultWithValue;
import org.example.planningservice.commons.result.ServiceError;
import org.springframework.http.ResponseEntity;

public class ResponseUtil {
    public static <T> ResponseEntity<ApiResponse<T>> fromResult(ResultWithValue<T> result) {
        if (result.isSuccess()) {
            return ResponseEntity.ok(new ApiResponse<>(true, "Success", result.getValue(), null));
        }

        ServiceError error = result.getError();
        return ResponseEntity.badRequest().body(
                new ApiResponse<>(false, error.getDescription(), null, error.getCode())
        );
    }
}
