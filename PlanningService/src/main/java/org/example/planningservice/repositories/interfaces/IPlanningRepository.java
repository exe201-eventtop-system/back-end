package org.example.planningservice.repositories.interfaces;

import org.example.planningservice.bo.entities.Planning;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.List;
import java.util.UUID;

public interface IPlanningRepository extends JpaRepository<Planning, UUID> {
    List<Planning> findAllByCustomerId(UUID customerId);
int countAllByCustomerId(UUID customerId);
}
