package org.example.planningservice.bo.entities;

import jakarta.persistence.*;
import org.example.planningservice.bo.eo.PlanningEnum;

import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.UUID;

@Entity
@Table(name = "planning")
public class Planning {
    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    @Column(name = "id", nullable = false)
    private UUID id;

    @Column(name = "customer_id", nullable = false)
    private UUID customerId;

    @Column(name = "name", length = 50, nullable = false)
    private String name;

    @Column(name = "description", columnDefinition = "text")
    private String description;
    @Column(name = "status")
    private PlanningEnum status;

    @Column(name = "location", columnDefinition = "text")
    private String location;

    @Column(name = "date_of_event")
    private LocalDateTime dateOfEvent;

    @Column(name = "budget", precision = 10, scale = 2)
    private BigDecimal budget;

    public PlanningEnum getStatus() {
        return status;
    }

    public void setStatus(PlanningEnum status) {
        this.status = status;
    }

    @Column(name = "about_number_people", length = 50)
    private int aboutNumberPeople;

    @Column(name = "main_color", length = 50)
    private String mainColor;

    @Column(name = "type_of_event", length = 50)
    private String typeOfEvent;

    @Column(name = "create_at")
    private LocalDateTime createAt;

    @Column(name = "update_date")
    private LocalDateTime updateDate;

    public UUID getId() {
        return id;
    }

    public void setId(UUID id) {
        this.id = id;
    }

    public UUID getCustomerId() {
        return customerId;
    }

    public void setCustomerId(UUID customerId) {
        this.customerId = customerId;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public String getDescription() {
        return description;
    }

    public void setDescription(String description) {
        this.description = description;
    }

    public String getLocation() {
        return location;
    }

    public void setLocation(String location) {
        this.location = location;
    }

    public LocalDateTime getDateOfEvent() {
        return dateOfEvent;
    }

    public void setDateOfEvent(LocalDateTime dateOfEvent) {
        this.dateOfEvent = dateOfEvent;
    }

    public BigDecimal getBudget() {
        return budget;
    }

    public void setBudget(BigDecimal budget) {
        this.budget = budget;
    }

    public int getAboutNumberPeople() {
        return aboutNumberPeople;
    }

    public void setAboutNumberPeople(int aboutNumberPeople) {
        this.aboutNumberPeople = aboutNumberPeople;
    }

    public String getMainColor() {
        return mainColor;
    }

    public void setMainColor(String mainColor) {
        this.mainColor = mainColor;
    }

    public String getTypeOfEvent() {
        return typeOfEvent;
    }

    public void setTypeOfEvent(String typeOfEvent) {
        this.typeOfEvent = typeOfEvent;
    }

    public LocalDateTime getCreateAt() {
        return createAt;
    }

    public void setCreateAt() {
        this.createAt = LocalDateTime.now();
    }



    public LocalDateTime getUpdateDate() {
        return updateDate;
    }

    public void setUpdateDate() {
        this.updateDate = LocalDateTime.now();
    }
}