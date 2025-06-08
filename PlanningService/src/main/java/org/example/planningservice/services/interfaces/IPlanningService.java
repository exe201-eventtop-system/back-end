package org.example.planningservice.services.interfaces;

import org.example.planningservice.bo.entities.Planning;
import org.example.planningservice.commons.dtos.PlanningStep1;
import org.example.planningservice.commons.dtos.PlanningStep2;
import org.example.planningservice.commons.helper.PaginationResult;
import org.example.planningservice.commons.result.ResultWithValue;

import java.util.List;
import java.util.UUID;

public interface IPlanningService {
    ResultWithValue<Planning> createStep1(PlanningStep1 step1, UUID userId);
    ResultWithValue<PaginationResult<Planning>> getAllPlans(int page, int size, String status,UUID userId);
    ResultWithValue<Integer> getNumberPlanning(UUID userId);
    ResultWithValue<Planning> getPlanById(UUID id);
    ResultWithValue<Planning> createStep2(PlanningStep2 step2);
    Planning createPlan(Planning planning);
    ResultWithValue<Void> deletePlan(UUID id);
}