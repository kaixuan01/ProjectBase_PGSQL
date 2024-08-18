import React, { useState, useEffect } from 'react';
import { useTable, useSortBy, useFilters, usePagination } from 'react-table';
import '../CSS/MyTable.css'; // Import the CSS file
import FuncHTTPReq from '../Common/funcHTTPReq';
import { buildQueryString } from '../Common/common';

const MyTable = ({ url, columns }) => {
    const [data, setData] = useState([]);
    const [totalItems, setTotalItems] = useState(0);
    
    const {
        getTableProps,
        getTableBodyProps,
        headerGroups,
        rows,
        prepareRow,
        state: { pageIndex, pageSize, sortBy, filters }, 
        setPageSize,
        // gotoPage,
        canPreviousPage,
        canNextPage,
        previousPage,
        nextPage,
    } = useTable(
        {
            columns,
            data,
            initialState: { pageIndex: 0, pageSize: 1 },
            manualPagination: true,
            manualSortBy: true,
            manualFilters: true,
        },
        useFilters,
        useSortBy,
        usePagination
    );
    console.log(pageIndex);
    console.log(sortBy);
    console.log(filters);
    console.log(url);
    console.log(pageSize);

    useEffect(() => {
        async function fetchData() {
            try {
                const queryParams = {
                    PageNumber: pageIndex + 1,
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
                        setTotalItems(data.totalCount); // Update total items count
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

    const firstItemIndex = pageIndex * pageSize + 1;
    const lastItemIndex = Math.min((pageIndex + 1) * pageSize, totalItems);

    return (
        <div className="table-container">
            <table {...getTableProps()} className="my-table">
                <thead>
                    {headerGroups.map((headerGroup) => (
                        <tr {...headerGroup.getHeaderGroupProps()} key={`row_${headerGroup.id}`}>
                            {headerGroup.headers.map((column) => (
                                <th
                                    {...column.getHeaderProps(column.getSortByToggleProps())}
                                    key={`col_${column.id}`}
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
                            <tr {...row.getRowProps()} key={`row_${row.id}`}>
                                {row.cells.map((cell) => (
                                    <td {...cell.getCellProps()} key={`cell_${cell.column.id}`}>
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
                    onChange={(e) => setPageSize(Number(e.target.value))}
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
                        {`${firstItemIndex}-${lastItemIndex} of ${totalItems}`}
                    </div>
                    <button onClick={() => previousPage()} disabled={!canPreviousPage} className="pagination-button">
                        {'<'}
                    </button>
                    <button onClick={() => nextPage()} disabled={!canNextPage} className="pagination-button">
                        {'>'}
                    </button>
                </div>
            </div>
        </div>
    );
};

export default MyTable;
