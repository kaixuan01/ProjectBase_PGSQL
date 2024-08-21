import MyTable from "../../../Control/MyTable"
import React, { useEffect, useState } from "react";
import { InputFilter, MultiSelectFilter } from "../../../Control/TableControl";
import { useFuncHTTPReq } from "../../../Hook/FuncHttpReq";
export default function UserListing() {
    const [userRole, setUserRole] = useState([]);

    console.log(userRole)
    const { FuncHTTPReq } = useFuncHTTPReq();


    useEffect(() => {
    async function fetchData() {
            await FuncHTTPReq({
                url: '/UserRole/GetRoleList',
                method: 'GET',
                onSuccess: (data) => {
                    setUserRole(data)
                }
            });
    }
    fetchData();
    }, []);


    const columns = React.useMemo(() => [
        {
            Header: 'Name',
            accessor: 'name',
            Filter: InputFilter,
        },
        {
            Header: 'Username',
            accessor: 'username',
            allowSort: true,
            Cell: ({ value }) => { return value }
        },
        {
            Header: 'Email',
            accessor: 'email',
            allowSort: true,
            Filter: InputFilter,
            disableFilters: true
        },
        {
            Header: 'Phone',
            accessor: 'phone',
            allowSort: true
        },
        {
            Header: 'User Role',
            accessor: 'role',
            allowSort: true,
            Filter: (props) => <MultiSelectFilter {...props} options={userRole} />,
            Cell: ({ value }) => { return value }
        }
    ], [userRole]);


    return <>
        <h1>User Listing</h1>
        <hr />
        <MyTable
            supportFliter
            url={'/User/GetUserList'}
            columns={columns}
        /></>
}