import React from 'react';
import { Result, Button } from 'antd';

const ServerError = () => {
  return (
    <Result
      status="500"
      title="500"
      subTitle="Sorry, something went wrong."
      extra={
        <Button type="primary" href="/admin/dashboard">
          Back to dashboard
        </Button>
      }
    />
  );
};

export default ServerError;
