import React from 'react';
import { Navbar, NavbarBrand, Button, Badge } from 'reactstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faUserCircle } from '@fortawesome/free-solid-svg-icons';

export default function MyTopBar({ onLogout }) {
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
        <Button color="secondary" onClick={onLogout}>
          Logout
        </Button>
      </div>
    </Navbar>
  );
}
