package org.example.planningservice.commons.dtos;

import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.Date;
import java.util.UUID;

public class PlanningStep2 extends PlanningStep1 {
public UUID id ;
public String name;
public String description;
  public    String   location;
   public LocalDateTime dateOfEvent;
   public BigDecimal    budget;
     public int  aboutNumberPeople;
      public String  mainColor;
       public String typeOfEvent;

    public PlanningStep2(String name, String description) {
        super(name, description);
    }

    public String getTypeOfEvent() {
        return typeOfEvent;
    }

    public PlanningStep2() {
        super();
    }

    public void setTypeOfEvent(String typeOfEvent) {
        this.typeOfEvent = typeOfEvent;
    }

    public String getMainColor() {
        return mainColor;
    }

    public void setMainColor(String mainColor) {
        this.mainColor = mainColor;
    }

    public int getAboutNumberPeople() {
        return aboutNumberPeople;
    }

    public void setAboutNumberPeople(int aboutNumberPeople) {
        this.aboutNumberPeople = aboutNumberPeople;
    }

    public BigDecimal getBudget() {
        return budget;
    }

    public BigDecimal setBudget(BigDecimal budget) {
        this.budget = budget;
        return budget;
    }

    public LocalDateTime getDateOfEvent() {
        return dateOfEvent;
    }

    public LocalDateTime setDateOfEvent(LocalDateTime dateOfEvent) {
        this.dateOfEvent = dateOfEvent;
        return dateOfEvent;
    }

    public String getLocation() {
        return location;
    }

    public void setLocation(String location) {
        this.location = location;
    }

    public UUID getId() {
        return id;
    }

    public void setId(UUID id) {
        this.id = id;
    }
}
