package org.example.planningservice.commons.helper;

import org.example.planningservice.bo.entities.Planning;

import java.util.ArrayList;
import java.util.List;

public class PaginationResult<T> {
    private List<T> content = new ArrayList<>();
    private int itemAmount;
    private int pageSize;
    private int currentPage;

    public PaginationResult() {
    }

    public PaginationResult(List<T> content, int itemAmount, int pageSize, int currentPage) {
        this.content = content;
        this.itemAmount = itemAmount;
        this.pageSize = pageSize;
        this.currentPage = currentPage;
    }


    public List<T> getContent() {
        return content;
    }

    public void setContent(List<T> content) {
        this.content = content;
    }

    public int getItemAmount() {
        return itemAmount;
    }

    public void setItemAmount(int itemAmount) {
        this.itemAmount = itemAmount;
    }

    public int getPageSize() {
        return pageSize;
    }

    public void setPageSize(int pageSize) {
        this.pageSize = pageSize;
    }

    public int getCurrentPage() {
        return currentPage;
    }

    public void setCurrentPage(int currentPage) {
        this.currentPage = currentPage;
    }

    public int getPageCount() {
        return (int) Math.ceil((double) itemAmount / pageSize);
    }
}
