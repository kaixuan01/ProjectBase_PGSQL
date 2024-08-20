import MyTable from "../../../Control/MyTable"
import React from "react";
import { DefaultColumnFilter } from "../../../Control/MyTable";
import { useSelector } from "react-redux";
export default function UserListing() {
    var isLogin = useSelector((state) => state.user);
    console.log(isLogin);
    const columns = React.useMemo(() => [
        {
            Header: 'ID',
            accessor: 'id',
            disableFilters: true,
            allowSort: true,
        },
        {
            Header: 'Name',
            accessor: 'name',
            disableFilters: true,
            allowSort: true
        },
        {
            Header: 'Username',
            accessor: 'userName',
            disableFilters: true,
            allowSort: true
        },
        {
            Header: 'Email',
            accessor: 'email',
            allowSort: true
        },
        {
            Header: 'Phone',
            accessor: 'phone',
            allowSort: true
        },
        {
            Header: 'User Role',
            accessor: 'userRole',
            allowFliter: true,
            allowSort: true,
            Cell: ({ value }) => { return value.description }
        }
    ], []);


    return <>
        <h1>User Listing</h1>
        <hr />
        <MyTable
            supportFliter
            url={'/User/GetUserList'}
            columns={columns}
        /></>
}