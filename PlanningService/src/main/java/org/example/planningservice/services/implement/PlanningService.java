package org.example.planningservice.services.implement;

import org.example.planningservice.bo.entities.Planning;
import org.example.planningservice.bo.eo.PlanningEnum;
import org.example.planningservice.commons.dtos.PlanningStep1;
import org.example.planningservice.commons.dtos.PlanningStep2;
import org.example.planningservice.commons.helper.PaginationResult;
import org.example.planningservice.commons.result.ResultWithValue;
import org.example.planningservice.commons.result.ServiceError;
import org.example.planningservice.repositories.interfaces.IPlanningRepository;
import org.example.planningservice.services.interfaces.IPlanningService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.*;

@Service
public class PlanningService implements IPlanningService {

    private final IPlanningRepository iPlanningRepository;
    @Autowired
    public PlanningService(IPlanningRepository iPlanningRepository) {
        this.iPlanningRepository = iPlanningRepository;
    }
    @Override
    public ResultWithValue<Planning> createStep1(PlanningStep1 step1, UUID userId) {
        try {
            Planning planning = new Planning();
            planning.setCustomerId(userId);
            planning.setCreateAt();
            planning.setName(step1.getName());
            planning.setStatus(PlanningEnum.Planning);
            planning.setDescription(step1.getDescription());
            return ResultWithValue.success(iPlanningRepository.save(planning));
        }catch (Exception e) {
            return ResultWithValue.failureWithValue(
                    ServiceError.unhandledException("Không thể tạo kế hoạch: " + e.getMessage())
            );
        }
    }

    @Override
    public ResultWithValue<PaginationResult<Planning>> getAllPlans(int page, int size, String status,UUID userId) {
        // Lấy toàn bộ danh sách từ repository (có lọc nếu cần)
        List<Planning> allPlans;
//        if (status != null && !status.isBlank()) {
//            allPlans = iPlanningRepository.findByStatus(status);
//        } else {
//            allPlans = iPlanningRepository.findAll();
//        }
         allPlans = iPlanningRepository.findAllByCustomerId(userId);
        allPlans.sort(Comparator.comparing(Planning::getCreateAt).reversed());
        // Tính toán phân trang
        int totalItems = allPlans.size();
        int currentPage = Math.max(page, 0); // Đảm bảo không âm

        int fromIndex = currentPage * size;
        int toIndex = Math.min(fromIndex + size, totalItems);

        List<Planning> paginatedPlans;
        if (fromIndex >= totalItems) {
            paginatedPlans = new ArrayList<>(); // Tránh lỗi chỉ số
        } else {
            paginatedPlans = allPlans.subList(fromIndex, toIndex);
        }

        // Tạo kết quả phân trang
        PaginationResult<Planning> paginationResult = new PaginationResult<>(
                paginatedPlans,
                totalItems,
                size,
                currentPage
        );

        return ResultWithValue.success(paginationResult);
    }

    @Override
    public ResultWithValue<Integer> getNumberPlanning(UUID userId) {
        int totalPlanning = iPlanningRepository.countAllByCustomerId(userId);
        return ResultWithValue.success(totalPlanning);
    }


    @Override
    public ResultWithValue<Planning> getPlanById(UUID id) {
        return iPlanningRepository.findById(id)
                .map(ResultWithValue::success)
                .orElseGet(() -> ResultWithValue.failureWithValue(
                        new ServiceError("PLANNING_NOT_FOUND", "Planning not found")
                ));
    }

    @Override
    public ResultWithValue<Planning> createStep2(PlanningStep2 step2) {
        Optional<Planning> optionalPlanning = iPlanningRepository.findById(step2.getId());
        Planning planning = optionalPlanning.get();
        planning.setLocation(step2.getLocation());
        planning.setUpdateDate();
        planning.setDateOfEvent(step2.getDateOfEvent());
        planning.setBudget(step2.getBudget());
        planning.setDescription(step2.getDescription());
        planning.setName(step2.getName());
        planning.setAboutNumberPeople(step2.getAboutNumberPeople());
        planning.setMainColor(step2.getMainColor());
        planning.setTypeOfEvent(step2.getTypeOfEvent());

        // Lưu lại
        Planning updatedPlanning = iPlanningRepository.save(planning);

        return ResultWithValue.success(updatedPlanning);
    }


    @Override
    public Planning createPlan(Planning planning) {
        return iPlanningRepository.save(planning);
    }

    @Override
    public ResultWithValue<Void> deletePlan(UUID id) {
        try {
            iPlanningRepository.deleteById(id);
            return ResultWithValue.success(null); // Vì kiểu là Void nên value = null
        } catch (Exception e) {
            return ResultWithValue.failureWithValue(
                    new ServiceError("DELETE_ERROR", "Xóa kế hoạch thất bại: " + e.getMessage())
            );
        }
    }

}
