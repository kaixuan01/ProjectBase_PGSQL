import React, { useState, useEffect, useMemo } from "react";
import MyTable from "../../../Control/MyTable";
import { InputFilter, MultiSelectFilter } from "../../../Control/TableControl";
import { useFuncHTTPReq } from "../../../Hook/FuncHttpReq";

export default function UserListing() {
    const [userRole, setUserRole] = useState([]);
    const { FuncHTTPReq } = useFuncHTTPReq();

    useEffect(() => {
        const fetchData = async () => {
            FuncHTTPReq({
                url: '/UserRole/GetRoleList',
                method: 'GET',
                onSuccess: (data) => {
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
    }, [FuncHTTPReq, userRole]);

    const columns = useMemo(() => [
        {
            Header: 'Info',
            accessor: 'name',
            Cell: ({ row }) => (
                <div>
                    {Object.entries(row.original).map(([key, value]) => (
                        <div key={key}>
                            <strong>{key}:</strong> {value}
                        </div>
                    ))}
                </div>
            ),
            disableSortBy: true,
        },
        {
            Header: 'Name - ID',
            accessor: '',
            Cell: ({ value, row }) => (
                <a href="/UserEdit">
                    {row.original.username} - {row.original.id}
                </a>
            ),
            disableSortBy: true,
        },
        {
            Header: 'Username',
            accessor: 'username'
        },
        {
            Header: 'Email',
            accessor: 'email',
            Filter: InputFilter,
            disableFilters: true,
        },
        {
            Header: 'Phone',
            accessor: 'phone',
        },
        {
            Header: 'User Role',
            accessor: 'role',
            Filter: ({ column }) => {console.log(1); return <MultiSelectFilter column={column} options={userRole} />},
        }
    ], [userRole]);

    const rowStyle = (row) => {
        return row.original.name && row.original.name.includes('1')
            ? { backgroundColor: 'LightYellow' }
            : {};
    };

    return (
        <>
            <h1>User Listing</h1>
            <hr />
            <MyTable 
                url={'/User/GetUserList'} 
                columns={columns}
                defaultSortBy={{
                    id: 'username',
                    desc: false,
                }}
                rowStyle={rowStyle}
             />
        </>
    );
}
