import React, { useState, useEffect } from 'react';
import { useTable, useSortBy, useFilters, usePagination } from 'react-table';
import '../CSS/MyTable.css'; // Import the CSS file
import FuncHTTPReq from '../Common/funcHTTPReq';
import {buildQueryString} from '../Common/common'
const MyTable = ({ fetchData,url, pageCount: controlledPageCount }) => {
    const [data, setData] = useState([]);
    const [columns, setColumns] =useState([]);
    const {
        getTableProps,
        getTableBodyProps,
        headerGroups,
        rows,
        prepareRow,
        state: { pageIndex, pageSize, sortBy, filters },
        setPageSize,
        gotoPage,
        setSortBy,
        setFilter,
        canPreviousPage,
        canNextPage,
        pageCount,
        previousPage,
        nextPage
    } = useTable(
        {
            columns,
            data,
            initialState: { pageIndex: 1, pageSize: 10 },
            manualPagination: true,
            manualSortBy: true,
            manualFilters: true,
            pageCount: controlledPageCount,
        },
        useFilters,
        useSortBy,
        usePagination
    );

    useEffect(() => {
        // Temporary columns
        setColumns([
            {
                Header: 'ID',
                accessor: 'id',
            },
            {
                Header: 'Name',
                accessor: 'name',
            },
            {
                Header: 'Age',
                accessor: 'age',
            },
            {
                Header: 'Country',
                accessor: 'country',
            },
        ]);

        // Temporary data
        setData([
            { id: 1, name: 'John Doe', age: 28, country: 'USA' },
            { id: 2, name: 'Jane Smith', age: 34, country: 'Canada' },
            { id: 3, name: 'Carlos Johnson', age: 45, country: 'Mexico' },
            { id: 4, name: 'Maria Garcia', age: 23, country: 'Spain' },
        ]);
    }, []);

    useEffect(() => {
        async function fetchData() {
            try {
                // Construct the query parameters object
                const queryParams = {
                    PageNumber: pageIndex,
                    PageSize: pageSize,
                    SortBy: sortBy,
                    ...filters // Assuming filters is an object with key-value pairs
                };

                // Build the query string
                const queryString = buildQueryString(queryParams);

                // Construct the full URL with query parameters
                const fullUrl = `${url}?${queryString}`;

                await FuncHTTPReq({
                    url: fullUrl,
                    method: 'GET',
                    onSuccess: (data) => {
                        setData(data);
                        console.log('Request succeeded with data:', data);
                    },
                    onError: (error) => {
                        console.error('Request failed with error:', error);
                    }
                });
            } catch (error) {
                console.error('Error in fetchData:', error);
            }
        }

        fetchData();
    }, [pageIndex, pageSize, sortBy, filters, url]);
    

    return (
        <div className="table-container">
            <table {...getTableProps()} className="my-table">
                <thead>
                    {headerGroups.map(headerGroup => (
                        <tr {...headerGroup.getHeaderGroupProps()} key={`row_${headerGroup.id}`}>
                            {headerGroup.headers.map(column => (
                                <th {...column.getHeaderProps(column.getSortByToggleProps())} key={`col_${column.id}`}>
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
                    {rows.map(row => {
                        prepareRow(row);
                        return (
                            <tr {...row.getRowProps()} key={`row_${row.id}`}>
                                {row.cells.map(cell => (
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
                <button onClick={() => gotoPage(0)} disabled={!canPreviousPage} className="pagination-button">
                    {'<<'}
                </button>
                <button onClick={() => gotoPage(pageIndex - 1)} disabled={!canPreviousPage} className="pagination-button">
                    {'<'}
                </button>
                <button onClick={() => gotoPage(pageIndex + 1)} disabled={!canNextPage} className="pagination-button">
                    {'>'}
                </button>
                <button onClick={() => gotoPage(pageCount - 1)} disabled={!canNextPage} className="pagination-button">
                    {'>>'}
                </button>
                <select
                    value={pageSize}
                    onChange={e => setPageSize(Number(e.target.value))}
                    className="page-size-selector"
                >
                    {[10, 20, 30, 40, 50].map(size => (
                        <option key={size} value={size}>
                            Show {size}
                        </option>
                    ))}
                </select>
            </div>
        </div>
    );
};

export default MyTable;
