import MyTable from "../../../Control/MyTable"
import React from "react";
export default function UserListing(){

    const columns = React.useMemo(() => [
        {
            Header: 'ID',
            accessor: 'id',
        },
        {
            Header: 'Name',
            accessor: 'name',
        },
        {
            Header: 'Username',
            accessor: 'userName',
        },
        {
            Header: 'Email',
            accessor: 'email',
        },
        {
            Header: 'Phone',
            accessor: 'phone',
        },
        {
            Header: 'User Role',
            accessor: 'userRole',
            Cell: ({value}) => value.description
        }
    ], []);


    return <>
    <h1>User Listing</h1>
    <hr/>
    <MyTable
        url={'/User/GetUserList'}
        columns={columns}
/></>
}