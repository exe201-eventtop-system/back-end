package org.example.planningservice.api.controller;
import io.swagger.v3.oas.annotations.Parameter;
import jakarta.servlet.http.HttpServletRequest;
import org.example.planningservice.bo.entities.Planning;
import org.example.planningservice.commons.dtos.PlanningStep1;
import org.example.planningservice.commons.dtos.PlanningStep2;
import org.example.planningservice.commons.helper.PaginationResult;
import org.example.planningservice.commons.helper.ResponseUtil;
import org.example.planningservice.commons.helper.jwt.JwtUtil;
import org.example.planningservice.commons.result.ApiResponse;
import org.example.planningservice.commons.result.ResultWithValue;
import org.example.planningservice.services.interfaces.IPlanningService;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.UUID;

@RestController
@RequestMapping("/api/planning")
public class PlanningController {
    private final IPlanningService iPlanningService;
private final JwtUtil  jwtUtil ;
    public PlanningController(IPlanningService iPlanningService, JwtUtil jwtUtil) {
        this.iPlanningService =iPlanningService ;
        this.jwtUtil = jwtUtil ;
    }

    @PostMapping("/step-1")
    public ResponseEntity<ApiResponse<Planning>> createStep1(@RequestBody PlanningStep1 planningStep1,HttpServletRequest request) {
        String token = request.getHeader("Authorization").replace("Bearer ", "");
        UUID userId = jwtUtil.extractUserId(token);
        return ResponseUtil.fromResult(iPlanningService.createStep1(planningStep1, userId));
    }
    @GetMapping
    public ResponseEntity<ApiResponse<PaginationResult<Planning>>> getAllPlanning(
            @RequestParam(defaultValue = "0") int page,
            @RequestParam(defaultValue = "10") int size,
            @RequestParam(required = false) String status,
            HttpServletRequest request
    ) {
        String token = request.getHeader("Authorization").replace("Bearer ", "");
        UUID userId = jwtUtil.extractUserId(token);
        System.out.println(userId);
        ResultWithValue<PaginationResult<Planning>> result = iPlanningService.getAllPlans(page, size, status,userId);
        return ResponseUtil.fromResult(result);
    }

    @DeleteMapping("{planningId}")
    public ResponseEntity<ApiResponse<Void>> deletePlanning(@PathVariable UUID planningId) {
        ResultWithValue<Void> result = iPlanningService.deletePlan(planningId);
        return ResponseUtil.fromResult(result);
    }
    @GetMapping("/{planningId}")
    public ResponseEntity<ApiResponse<Planning>> getPlanning(
            @PathVariable UUID planningId
    ) {
        ResultWithValue<Planning> result = iPlanningService.getPlanById(planningId);
        return ResponseUtil.fromResult(result);
    }
    @GetMapping("/number"   )
    public ResponseEntity<ApiResponse<Integer>> getNumberPlanning(
            HttpServletRequest request
    ) {
        String token = request.getHeader("Authorization").replace("Bearer ", "");
        UUID userId = jwtUtil.extractUserId(token);
        ResultWithValue<Integer> result = iPlanningService.getNumberPlanning(userId);
        return ResponseUtil.fromResult(result);
    }
    @PutMapping("/step-2")
    public ResponseEntity<ApiResponse<Planning>> createStep1(@RequestBody PlanningStep2 planningStep2) {

        ResultWithValue<Planning> result = iPlanningService.createStep2(planningStep2);
        return ResponseUtil.fromResult(result);
    }
}
