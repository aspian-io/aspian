import React from 'react';
import { Result, Button } from 'antd';

const NotFound = () => {
  return (
    <Result
      status="404"
      title="404"
      subTitle="Sorry, the page you visited does not exist."
      extra={
        <Button type="primary" href="/admin/dashboard">
          Back to dashboard
        </Button>
      }
    />
  );
};

export default NotFound;
