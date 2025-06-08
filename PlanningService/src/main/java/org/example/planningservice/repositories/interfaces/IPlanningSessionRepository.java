package org.example.planningservice.repositories.interfaces;

import org.example.planningservice.bo.entities.PlanningSession;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.UUID;

public interface IPlanningSessionRepository extends JpaRepository<PlanningSession, UUID> {}
