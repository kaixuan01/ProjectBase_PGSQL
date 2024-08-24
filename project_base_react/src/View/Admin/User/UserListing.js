import React, { useState, useEffect, useMemo } from "react";
import MyTable from "../../../Control/MyTable";
import { InputFilter, MultiSelectFilter } from "../../../Control/TableControl";
import { useFuncHTTPReq } from "../../../Hook/FuncHttpReq";

export default function UserListing() {
    const [userRole, setUserRole] = useState([]);
    const { FuncHTTPReq } = useFuncHTTPReq();

    useEffect(() => {
        const fetchData = async () => {
            await FuncHTTPReq({
                url: '/UserRole/GetRoleList',
                method: 'GET',
                onSuccess: (data) => {
                    // Only set state if data has changed to avoid infinite loop
                    if (data && JSON.stringify(data) !== JSON.stringify(userRole)) {
                        setUserRole(data);
                    }
                },
                onError: (error) => {
                    console.error("Error fetching roles:", error);
                }
            });
        };

        fetchData();
    }, [FuncHTTPReq, userRole]); // Remove userRole from dependency array

    const columns = useMemo(() => [
        {
            Header: 'Name',
            accessor: 'name',
            Filter: InputFilter,
        },
        {
            Header: 'Username',
            accessor: 'username',
            allowSort: true,
        },
        {
            Header: 'Email',
            accessor: 'email',
            allowSort: true,
            Filter: InputFilter,
            disableFilters: true,
        },
        {
            Header: 'Phone',
            accessor: 'phone',
            allowSort: true,
        },
        {
            Header: 'User Role',
            accessor: 'role',
            allowSort: true,
            Filter: ({ column }) => <MultiSelectFilter column={column} options={userRole} />,
        }
    ], [userRole]); // Keep userRole only in memoized columns

    return (
        <>
            <h1>User Listing</h1>
            <hr />
            <MyTable url={'/User/GetUserList'} columns={columns} />
        </>
    );
}
