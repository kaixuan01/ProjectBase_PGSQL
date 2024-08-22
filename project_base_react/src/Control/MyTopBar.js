import React, { useCallback } from 'react';
import { Navbar, NavbarBrand, Button } from 'reactstrap';
import HTTPReq from '../Control/HTTPReq';
import { useAuthHandlers } from '../Hook/AuthHandlers';
import styles from '../CSS/TopBar.module.css'; // Import CSS module

export default function MyTopBar() {
  const { handleLogout, userName } = useAuthHandlers(); // Assuming useAuthHandlers provides userName

  const successLogout = useCallback(() => {
    handleLogout();
  }, [handleLogout]);

  return (
    <Navbar className={styles.navbar} dark expand="md" fixed="top">
      <NavbarBrand href="/" className={styles.brand}>
        Project <span className={styles.brandSpan}>Base</span>
      </NavbarBrand>
      <div className="ml-auto d-flex align-items-center">
        <span className={styles.welcomeMessage}>Welcome, {userName}!</span>
        <img 
          src="/images/Winnie.png" 
          alt="User Icon" 
          className={styles.userIcon} 
        />
        <HTTPReq
          method="POST"
          url={`/OAuth/Logout`}
          credentials="include"
          onSuccess={(result) => successLogout(result)}
        >
          {({ sendRequest }) => (
            <Button color='danger' className={styles.logoutButton} onClick={sendRequest}>
              Logout
            </Button>
          )}
        </HTTPReq>
      </div>
    </Navbar>
  );
}
