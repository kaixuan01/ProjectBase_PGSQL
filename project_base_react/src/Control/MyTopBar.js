import React, { useCallback } from 'react';
import { Navbar, NavbarBrand, Button, Badge } from 'reactstrap';
import HTTPReq from '../Control/HTTPReq';
import { showSuccessAlert } from '../Common/common';

export default function MyTopBar({ onLogout }) {

  const successLogout = useCallback((result) => {
    // showSuccessAlert(result.message);
    onLogout();
  }, []);

  return (
    <Navbar color="dark" dark expand="md" className="px-3">
    <NavbarBrand href="/">
        Project <span style={{ backgroundColor: '#f7ab1e', color: 'black', padding: '2px 6px', borderRadius: '4px' }}>Base</span>
    </NavbarBrand>
      <div className="ml-auto d-flex align-items-center">
      <img 
          src="/images/Winnie.png" 
          alt="User Icon" 
          style={{ 
            width: '45px', 
            height: '45px', 
            marginRight: '12px', 
            borderRadius: '50%', 
            objectFit: 'cover' 
          }} 
        />
        <HTTPReq
            method="POST"
            url={`/OAuth/Logout`}
            credentials='include'
            onSuccess={(result) => successLogout(result)} >
            {({ sendRequest }) => (
                <Button color="secondary" onClick={sendRequest}>
                  Logout
                </Button>
            )}
        </HTTPReq>
            
      </div>
    </Navbar>
  );
}
