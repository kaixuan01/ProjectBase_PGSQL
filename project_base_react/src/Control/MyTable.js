import React, { useState, useEffect } from 'react';
import { useTable, useSortBy, useFilters, usePagination } from 'react-table';
import '../CSS/MyTable.css'; // Import the CSS file
import FuncHTTPReq from '../Common/funcHTTPReq';
import { buildQueryString } from '../Common/common';

const MyTable = ({ url, columns }) => {
    const [data, setData] = useState([]);
    const [totalItems, setTotalItems] = useState(0);
    const [pageIndex, setPageIndex] = useState(1);
    const [itemQty, setItemQty] = useState(0);
    const {
        getTableProps,
        getTableBodyProps,
        headerGroups,
        rows,
        prepareRow,
        state: { pageSize, sortBy, filters },
        setPageSize,
    } = useTable(
        {
            columns,
            data,
            initialState: { pageSize: 1 },
            manualPagination: true,
            manualSortBy: true,
            manualFilters: true,
            rowCount: totalItems,
        },
        useFilters,
        useSortBy,
        usePagination
    );
    useEffect(() => {
        async function fetchData() {
            try {
                const queryParams = {
                    PageNumber: pageIndex,
                    PageSize: pageSize,
                    SortBy: sortBy,
                    ...filters,
                };

                // Build the query string
                const queryString = buildQueryString(queryParams);

                // Construct the full URL with query parameters
                const fullUrl = `${url}?${queryString}`;

                await FuncHTTPReq({
                    url: fullUrl,
                    method: 'GET',
                    onSuccess: (data) => {
                        setData(data.items);
                        setTotalItems(data.totalCount);
                        setItemQty(data.items.length);
                    },
                    onError: (error) => {
                        console.error('Request failed with error:', error);
                    },
                });
            } catch (error) {
                console.error('Error in fetchData:', error);
            }
        }
        fetchData();
    }, [pageIndex, pageSize, sortBy, filters, url]);

    const handlePageChange = (newPageIndex) => {
        if (newPageIndex >= 1 && newPageIndex <= Math.ceil(totalItems / pageSize)) {
            setPageIndex(newPageIndex);
        }
    }
    
    const totalPages = Math.ceil(totalItems / pageSize);
    const firstItemIndex = (pageIndex - 1) * pageSize + 1;
    const lastItemIndex = Math.min(pageIndex * pageSize, totalItems);


    const handlePageSizeChange = (newPageSize) => {
        setPageSize(newPageSize);
        setPageIndex(1)
    };
    return (
        <div className="table-container">
            <table {...getTableProps()} className="my-table">
                <thead>
                    {headerGroups.map((headerGroup) => (
                        <tr {...headerGroup.getHeaderGroupProps()} key={`header-${headerGroup.id}`}>
                            {headerGroup.headers.map((column) => (
                                <th
                                    {...column.getHeaderProps(column.getSortByToggleProps())}
                                    key={`column-${column.id}`}
                                >
                                    {column.render('Header')}
                                    <span>
                                        {column.isSorted
                                            ? column.isSortedDesc
                                                ? ' 🔽'
                                                : ' 🔼'
                                            : ''}
                                    </span>
                                </th>
                            ))}
                        </tr>
                    ))}
                </thead>
                <tbody {...getTableBodyProps()}>
                    {rows.map((row) => {
                        prepareRow(row);
                        return (
                            <tr {...row.getRowProps()} key={`row-${row.id}`}>
                                {row.cells.map((cell) => (
                                    <td {...cell.getCellProps()} key={`cell-${cell.column.id}`}>
                                        {cell.render('Cell')}
                                    </td>
                                ))}
                            </tr>
                        );
                    })}
                </tbody>
            </table>
            <div className="pagination-controls">
                <span>Show Rows per page</span>
                <select
                    value={pageSize}
                    onChange={(e) => handlePageSizeChange(Number(e.target.value))}
                    className="page-size-selector"
                >
                    {[1, 2, 3, 4, 5].map((size) => (
                        <option key={size} value={size}>
                            {size}
                        </option>
                    ))}
                </select>
                <div className="pagination-buttons">
                    <div className="rows-info">
                        {firstItemIndex}-{lastItemIndex} of {totalItems}
                    </div>
                    <button 
                        onClick={() => handlePageChange(pageIndex - 1)} 
                        disabled={pageIndex === 1} 
                        className="pagination-button"
                    >
                        {'<'}
                    </button>
                    <button 
                        onClick={() => handlePageChange(pageIndex + 1)} 
                        disabled={(pageIndex) * pageSize >= totalItems} 
                        className="pagination-button"
                    >
                        {'>'}
                    </button>
                </div>
            </div>
        </div>
    );
};

export default MyTable;
