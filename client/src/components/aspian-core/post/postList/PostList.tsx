import React, {
  useState,
  ReactText,
  FC,
  useEffect,
  Fragment,
  MouseEvent,
  useContext,
} from 'react';
import {
  Table,
  Button,
  Space,
  Tooltip,
  Row,
  Col,
  Typography,
  Popconfirm,
  Input,
  DatePicker,
  Slider,
} from 'antd';
import { TableRowSelection, ColumnsType } from 'antd/lib/table/interface';
import { RangeValue, EventValue } from 'rc-picker/lib/interface';
import {
  EditFilled,
  DeleteFilled,
  SearchOutlined,
  CalendarFilled,
  ControlOutlined,
  FilterOutlined,
  EyeFilled,
} from '@ant-design/icons';
import '../../../../scss/aspian-core/pages/posts/all-posts/_all-posts.scss';
import {
  ITaxonomyPost,
  TaxonomyTypeEnum,
  PostStatusEnum,
} from '../../../../app/models/aspian-core/post';
import { CheckOutlined, CloseOutlined } from '@ant-design/icons';
import moment, { Moment } from 'moment';
import jalaliMoment from 'jalali-moment';
import { WithTranslation, withTranslation } from 'react-i18next';
import Title from 'antd/lib/typography/Title';
import Paragraph from 'antd/lib/typography/Paragraph';
import Text from 'antd/lib/typography/Text';
import {
  LanguageActionTypeEnum,
  DirectionActionTypeEnum,
} from '../../../../app/stores/aspian-core/locale/types';
import { SorterResult, ColumnType } from 'antd/es/table/interface';
import { UAParser } from 'ua-parser-js';
import Highlighter from 'react-highlight-words';

import 'react-modern-calendar-datepicker/lib/DatePicker.css';
import PesianDatePicker, { DayRange } from 'react-modern-calendar-datepicker';
import '../../../../scss/aspian-core/components/modern-calendar/_persian-datepicker.scss';
import { e2p } from '../../../../js/aspian-core/base/numberConverter';
import PostStore from '../../../../app/stores/aspian-core/post/postStore';
import { observer } from 'mobx-react-lite';
import LocaleStore from '../../../../app/stores/aspian-core/locale/localeStore';
import { Link } from 'react-router-dom';
import { history } from '../../../..';

interface IPostAntdTable {
  key: string;
  title: JSX.Element;
  postCategory: (JSX.Element | '')[];
  postStatus: any;
  postAttachments: number;
  commentAllowed: JSX.Element;
  viewCount: number;
  pinned: JSX.Element;
  comments: number;
  childPosts: number;
  createdAt: string;
  createdBy: string;
  modifiedAt: string;
  modifiedBy: string;
  device: string | undefined;
  os: string | undefined;
  browser: string | undefined;
  userIPAddress: string;
}

const { RangePicker } = DatePicker;

const PostList: FC<WithTranslation> = ({ t }) => {
  // Constants
  /// Default page size
  const DFAULT_PAGE_SIZE = 10;
  /// Columns dataIndexes
  const TITLE = 'title';
  const CATEGORY = 'postCategory';
  const STATUS = 'postStatus';
  const ATTACHMENTS = 'postAttachments';
  const COMMENT_ALLOWED = 'commentAllowed';
  const VIEW_COUNT = 'viewCount';
  const PINNED = 'pinned';
  const COMMENTS = 'comments';
  const CHILD_POSTS = 'childPosts';
  const CREATED_AT = 'createdAt';
  const CREATED_BY = 'createdBy';
  const MODIFIED_AT = 'modifiedAt';
  const MODIFIED_BY = 'modifiedBy';
  const USER_AGENT = 'userAgent';
  const USER_AGENT_DEVICE = 'device';
  const USER_AGENT_OS = 'os';
  const USER_AGENT_BROWSER = 'browser';
  const IP_ADDRESS = 'userIPAddress';
  const ACTIONS = 'actions';
  // Colomn arrays with different types of filters
  /// Columns with search filter
  const SEARCH_FILTERED_COLUMNS: string[] = [
    TITLE,
    CATEGORY,
    STATUS,
    COMMENT_ALLOWED,
    PINNED,
    IP_ADDRESS,
    CREATED_BY,
    MODIFIED_BY,
    USER_AGENT_DEVICE,
    USER_AGENT_OS,
    USER_AGENT_BROWSER,
  ];
  /// Columns with DateRange filter
  const DATERANGE_FILTERED_COLUMNS: string[] = [CREATED_AT, MODIFIED_AT];
  /// Columns with slider filter
  const SLIDER_FILTERED_COLUMNS: string[] = [
    ATTACHMENTS,
    VIEW_COUNT,
    COMMENTS,
    CHILD_POSTS,
  ];

  // Stores
  const postStore = useContext(PostStore);
  const localeStore = useContext(LocaleStore);

  // UseStates
  const [currentPage, setCurrentPage] = useState(1);
  const [selectedRowKeys, setSelectedRowKeys] = useState<ReactText[]>([]);
  const [deleteRangeBtn, setDeleteRangeBtn] = useState(true);
  const [searchText, setSearchText] = useState<React.ReactText>('');
  const [dateRange, setDateRange] = useState<
    [EventValue<Moment>, EventValue<Moment>]
  >([null, null]);
  const [searchedColumn, setSearchedColumn] = useState<
    string | number | React.ReactText[] | undefined
  >('');
  const [windowWidth, setWindowWidth] = useState(window.innerWidth);

  const [selectedDayRange, setSelectedDayRange] = useState<DayRange>({
    from: null,
    to: null,
  });
  const [targetBtn, setTargetBtn] = useState('');

  // On select a row event
  const onSelectChange = (selectedRowKeys: ReactText[]) => {
    selectedRowKeys.length > 0
      ? setDeleteRangeBtn(false)
      : setDeleteRangeBtn(true);
    setSelectedRowKeys(selectedRowKeys);
  };

  // Row selection functionality implementation
  const rowSelection: TableRowSelection<object> = {
    selectedRowKeys,
    onChange: onSelectChange,
    selections: [
      Table.SELECTION_ALL,
      Table.SELECTION_INVERT,
      {
        key: 'odd',
        text: t('post-list.row-selection-menu.items.select-odd-row'),
        onSelect: (changableRowKeys: ReactText[]) => {
          let newSelectedRowKeys = [];
          newSelectedRowKeys = changableRowKeys.filter((key, index) => {
            if (index % 2 !== 0) {
              return false;
            }
            return true;
          });
          setSelectedRowKeys(newSelectedRowKeys);
        },
      },
      {
        key: 'even',
        text: t('post-list.row-selection-menu.items.select-even-row'),
        onSelect: (changableRowKeys: ReactText[]) => {
          let newSelectedRowKeys = [];
          newSelectedRowKeys = changableRowKeys.filter((key, index) => {
            if (index % 2 !== 0) {
              return true;
            }
            return false;
          });
          setSelectedRowKeys(newSelectedRowKeys);
        },
      },
    ],
  };

  // Custom slider filter functionality implementation
  const getColumnSearchPropsForSliderFilter = (
    dataIndex: string,
    maxNumber: number
  ): ColumnType<any> => ({
    filterDropdown: ({ setSelectedKeys, confirm, clearFilters }) => (
      <div style={{ padding: 8 }}>
        <Slider
          range
          step={1}
          max={maxNumber + 50}
          disabled={maxNumber === 0}
          defaultValue={[0, maxNumber > 20 ? maxNumber : 30]}
          onAfterChange={(value) => {
            if (value) {
              setSelectedKeys([value[0], value[1]]);
            }
          }}
        />
        <div>
          <div style={{ marginTop: '10px' }}>
            <Space>
              <Button
                type="primary"
                onClick={() => handleSearchDateRange(confirm, dataIndex)}
                icon={<FilterOutlined />}
                size="small"
                style={{ width: 90 }}
              >
                {t('post-list.table.filters.buttons.filter')}
              </Button>
              <Button
                onClick={() => handleReset(clearFilters)}
                size="small"
                style={{ width: 90 }}
              >
                {t('post-list.table.filters.buttons.reset')}
              </Button>
            </Space>
          </div>
        </div>
      </div>
    ),
    filterIcon: (filtered) => (
      <ControlOutlined
        style={{ color: filtered ? '#1890ff' : undefined, fontSize: '13px' }}
      />
    ),
  });

  // Custom date range filter functionality implementation
  const getColumnSearchPropsForDateRangeFilter = (
    dataIndex: string
  ): ColumnType<any> => ({
    filterDropdown: ({ setSelectedKeys, confirm, clearFilters }) => (
      <div style={{ padding: 8 }}>
        {localeStore.lang === LanguageActionTypeEnum.fa && (
          <Fragment>
            <PesianDatePicker
              inputPlaceholder="از تاریخ --- تا تاریخ"
              value={selectedDayRange}
              onChange={setSelectedDayRange}
              shouldHighlightWeekends
              locale="fa"
              calendarPopperPosition="bottom"
              calendarClassName="persian-datepicker"
              inputClassName="persian-datepicker-input"
            />

            <div>
              <div style={{ marginTop: '10px' }}>
                <Space>
                  <Button
                    type="primary"
                    onClick={() => {
                      const fromDay = selectedDayRange.from?.day;
                      const fromMonth = selectedDayRange.from?.month;
                      const fromYear = selectedDayRange.from?.year;

                      const toDay = selectedDayRange.to?.day;
                      const toMonth = selectedDayRange.to?.month;
                      const toYear = selectedDayRange.to?.year;

                      const fromInput = `${fromYear}/${fromMonth}/${fromDay}`;
                      const toInput = `${toYear}/${toMonth}/${toDay}`;

                      const from = jalaliMoment
                        .from(fromInput, 'fa', 'YYYY/MM/DD')
                        .locale('en')
                        .format('YYYY/MM/DD');

                      const to = jalaliMoment
                        .from(toInput, 'fa', 'YYYY/MM/DD')
                        .locale('en')
                        .format('YYYY/MM/DD');

                      setSelectedKeys([from, to]);

                      confirm();
                      setSearchedColumn(dataIndex);
                    }}
                    icon={<SearchOutlined />}
                    size="small"
                    style={{ width: 90 }}
                  >
                    {t('post-list.table.filters.buttons.search')}
                  </Button>
                  <Button
                    onClick={() => {
                      clearFilters!();
                      setSelectedDayRange({ from: null, to: null });
                      postStore.loadPosts(DFAULT_PAGE_SIZE, 0);
                    }}
                    size="small"
                    style={{ width: 90 }}
                  >
                    {t('post-list.table.filters.buttons.reset')}
                  </Button>
                </Space>
              </div>
            </div>
          </Fragment>
        )}

        {localeStore.lang === LanguageActionTypeEnum.en && (
          <Fragment>
            <RangePicker
              value={dateRange}
              inputReadOnly={true}
              format="YYYY/MM/DD"
              onChange={(
                dates: RangeValue<Moment>,
                dateStrings: [string, string]
              ) => {
                if (dates) {
                  setDateRange([dates![0], dates![1]]);
                  setSelectedKeys([
                    dates![0]?.format('YYYY/MM/DD') as ReactText,
                    dates![1]?.format('YYYY/MM/DD') as ReactText,
                  ]);
                }
              }}
            />

            <div>
              <div style={{ marginTop: '10px' }}>
                <Space>
                  <Button
                    type="primary"
                    onClick={() => handleSearchDateRange(confirm, dataIndex)}
                    icon={<SearchOutlined />}
                    size="small"
                    style={{ width: 90 }}
                  >
                    {t('post-list.table.filters.buttons.search')}
                  </Button>
                  <Button
                    onClick={() => handleReset(clearFilters)}
                    size="small"
                    style={{ width: 90 }}
                  >
                    {t('post-list.table.filters.buttons.reset')}
                  </Button>
                </Space>
              </div>
            </div>
          </Fragment>
        )}
      </div>
    ),
    filterIcon: (filtered) => (
      <CalendarFilled style={{ color: filtered ? '#1890ff' : undefined }} />
    ),
  });

  const handleSearchDateRange = (
    confirm: () => void,
    dataIndex: string | number | React.ReactText[] | undefined
  ) => {
    confirm();
    setSearchedColumn(dataIndex);
  };

  let searchInput: Input;
  // Custom filter functionality implementation
  const getColumnSearchPropsForSearchFilter = (
    dataIndex: string
  ): ColumnType<any> => ({
    filterDropdown: ({
      setSelectedKeys,
      selectedKeys,
      confirm,
      clearFilters,
    }) => (
      <div style={{ padding: 8 }}>
        <Input
          ref={(node) => {
            searchInput = node!;
          }}
          placeholder={t('post-list.table.filters.inputs.search.placeholder')}
          value={selectedKeys[0]}
          onChange={(e) =>
            setSelectedKeys(e.target.value ? [e.target.value] : [])
          }
          onPressEnter={() => handleSearch(selectedKeys, confirm, dataIndex)}
          style={{ width: 188, marginBottom: 8, display: 'block' }}
        />
        <Space>
          <Button
            type="primary"
            onClick={() => handleSearch(selectedKeys, confirm, dataIndex)}
            icon={<SearchOutlined />}
            size="small"
            style={{ width: 90 }}
          >
            {t('post-list.table.filters.buttons.search')}
          </Button>
          <Button
            onClick={() => handleReset(clearFilters)}
            size="small"
            style={{ width: 90 }}
          >
            {t('post-list.table.filters.buttons.reset')}
          </Button>
        </Space>
      </div>
    ),
    filterIcon: (filtered) => (
      <SearchOutlined style={{ color: filtered ? '#1890ff' : undefined }} />
    ),
    onFilterDropdownVisibleChange: (visible) => {
      if (visible) {
        setTimeout(() => searchInput.select(), 100);
      }
    },
    render: (text: React.ReactText) =>
      searchedColumn === dataIndex ? (
        <Highlighter
          highlightStyle={{ backgroundColor: '#ffc069', padding: 0 }}
          searchWords={[searchText.toString()]}
          autoEscape
          textToHighlight={
            text ? text.toString().replace(new RegExp(',+$'), '') : ''
          }
        />
      ) : (
        text
      ),
  });

  const handleSearch = (
    selectedKeys: React.ReactText[],
    confirm: () => void,
    dataIndex: string | number | React.ReactText[] | undefined
  ) => {
    confirm();
    setSearchText(selectedKeys[0]);
    setSearchedColumn(dataIndex);
  };

  const handleReset = (clearFilters: (() => void) | undefined) => {
    clearFilters!();
    setSearchText('');
    setDateRange([null, null]);
    postStore.loadPosts(DFAULT_PAGE_SIZE, 0);
  };

  const columns: ColumnsType<IPostAntdTable> = [
    {
      title: t('post-list.table.thead.title'),
      width: 200,
      dataIndex: TITLE,
      fixed: windowWidth > 576 ? 'left' : undefined,
      sorter: true,
      ...getColumnSearchPropsForSearchFilter(TITLE),
    },
    {
      title: t('post-list.table.thead.category'),
      width: 200,
      dataIndex: CATEGORY,
      ellipsis: true,
      sorter: true,
      ...getColumnSearchPropsForSearchFilter(CATEGORY),
    },
    {
      title: t('post-list.table.thead.status'),
      width: 100,
      dataIndex: STATUS,
      align: 'center',
      sorter: true,
      filterMultiple: false,
      filters: [
        {
          text: t('post-list.table.filters.post-status.publish'),
          value: PostStatusEnum.Publish,
        },
        {
          text: t('post-list.table.filters.post-status.pending'),
          value: PostStatusEnum.Pending,
        },
        {
          text: t('post-list.table.filters.post-status.draft'),
          value: PostStatusEnum.Draft,
        },
        {
          text: t('post-list.table.filters.post-status.auto-draft'),
          value: PostStatusEnum.AutoDraft,
        },
        {
          text: t('post-list.table.filters.post-status.future'),
          value: PostStatusEnum.Future,
        },
        {
          text: t('post-list.table.filters.post-status.inherit'),
          value: PostStatusEnum.Inherit,
        },
        {
          text: t('post-list.table.filters.post-status.private'),
          value: PostStatusEnum.Private,
        },
        {
          text: t('post-list.table.filters.post-status.trash'),
          value: PostStatusEnum.Trash,
        },
      ],
    },
    {
      title: t('post-list.table.thead.attachments'),
      width: 130,
      dataIndex: ATTACHMENTS,
      align: 'center',
      sorter: true,
      ...getColumnSearchPropsForSliderFilter(
        ATTACHMENTS,
        postStore.maxAttachmentsNumber
      ),
    },
    {
      title: t('post-list.table.thead.comment-allowed'),
      width: 200,
      dataIndex: COMMENT_ALLOWED,
      align: 'center',
      sorter: true,
      filterMultiple: false,
      filters: [
        {
          text: t('post-list.table.filters.comment-allowed.allowed'),
          value: true,
        },
        {
          text: t('post-list.table.filters.comment-allowed.not-allowed'),
          value: false,
        },
      ],
    },
    {
      title: t('post-list.table.thead.view-count'),
      width: 200,
      dataIndex: VIEW_COUNT,
      align: 'center',
      sorter: true,
      ...getColumnSearchPropsForSliderFilter(
        VIEW_COUNT,
        postStore.maxViewCount
      ),
    },
    {
      title: t('post-list.table.thead.pinned'),
      width: 100,
      dataIndex: PINNED,
      align: 'center',
      sorter: true,
      filterMultiple: false,
      filters: [
        {
          text: t('post-list.table.filters.is-pinned.pinned'),
          value: true,
        },
        {
          text: t('post-list.table.filters.is-pinned.not-pinned'),
          value: false,
        },
      ],
    },
    {
      title: t('post-list.table.thead.comments'),
      width: 120,
      dataIndex: COMMENTS,
      align: 'center',
      sorter: true,
      ...getColumnSearchPropsForSliderFilter(COMMENTS, postStore.maxComments),
    },
    {
      title: t('post-list.table.thead.child-posts'),
      width: 150,
      dataIndex: CHILD_POSTS,
      align: 'center',
      sorter: true,
      ...getColumnSearchPropsForSliderFilter(
        CHILD_POSTS,
        postStore.maxChildPosts
      ),
    },
    {
      title: t('post-list.table.thead.created-at'),
      width: 200,
      dataIndex: CREATED_AT,
      align: 'center',
      sorter: true,
      ...getColumnSearchPropsForDateRangeFilter(CREATED_AT),
    },
    {
      title: t('post-list.table.thead.created-by'),
      width: 150,
      dataIndex: CREATED_BY,
      sorter: true,
      ...getColumnSearchPropsForSearchFilter(CREATED_BY),
    },
    {
      title: t('post-list.table.thead.modified-at'),
      width: 150,
      dataIndex: MODIFIED_AT,
      align: 'center',
      sorter: true,
      ...getColumnSearchPropsForDateRangeFilter(MODIFIED_AT),
    },
    {
      title: t('post-list.table.thead.modified-by'),
      width: 150,
      dataIndex: MODIFIED_BY,
      sorter: true,
      ...getColumnSearchPropsForSearchFilter(MODIFIED_BY),
    },
    {
      title: t('post-list.table.thead.user-agent.name'),

      dataIndex: USER_AGENT,
      ellipsis: true,
      children: [
        {
          title: t('post-list.table.thead.user-agent.sub-items.device'),
          dataIndex: USER_AGENT_DEVICE,
          align: 'center',
          width: 100,
          filterMultiple: false,
          filters: [
            {
              text: 'Desktop',
              value: 'desktop',
            },
            {
              text: 'Tablet',
              value: 'tablet',
            },
            {
              text: 'Mobile',
              value: 'mobile',
            },
          ],
        },
        {
          title: t('post-list.table.thead.user-agent.sub-items.os'),
          dataIndex: USER_AGENT_OS,
          align: 'center',
          width: 150,
          filterMultiple: false,
          filters: [
            {
              text: 'macOS',
              value: 'macOS',
            },
            {
              text: 'Windows',
              value: 'windows',
            },
            {
              text: 'Linux',
              value: 'linux',
            },
            {
              text: 'iPadOS',
              value: 'iPadOS',
            },
            {
              text: 'iPhoneOS',
              value: 'iPhoneOS',
            },
            {
              text: 'Android',
              value: 'android',
            },
          ],
        },
        {
          title: t('post-list.table.thead.user-agent.sub-items.browser'),
          dataIndex: USER_AGENT_BROWSER,
          align: 'center',
          width: 170,
          filterMultiple: false,
          filters: [
            {
              text: 'Chrome',
              value: 'chrome',
            },
            {
              text: 'Safari',
              value: 'safari',
            },
            {
              text: 'Firefox',
              value: 'firefox',
            },
            {
              text: 'Edge',
              value: 'edge',
            },
            {
              text: 'Internet Explorer',
              value: 'IE',
            },
            {
              text: 'Opera',
              value: 'opera',
            },
          ],
        },
      ],
    },
    {
      title: t('post-list.table.thead.ip-address'),
      width: 150,
      dataIndex: IP_ADDRESS,
      sorter: true,
      ...getColumnSearchPropsForSearchFilter(IP_ADDRESS),
    },
    {
      title: t('post-list.table.thead.actions'),
      key: ACTIONS,
      fixed: windowWidth > 576 ? 'right' : undefined,
      width: 150,
      align: 'center',
      render: (text, record, index) => (
        <Space>
          <Tooltip title={t('post-list.table.tooltip.view-post')} color="gray">
            <Button
              onClick={() => history.push(`/admin/posts/details/${record.key}`)}
              type="text"
              size="middle"
              icon={<EyeFilled />}
              className="text primary-color"
            />
          </Tooltip>
          <Tooltip title={t('post-list.table.tooltip.edit-post')} color="gray">
            <Button
              type="text"
              size="middle"
              icon={<EditFilled />}
              className="text warning-color"
            />
          </Tooltip>
          <Tooltip
            title={t('post-list.table.tooltip.delete-post')}
            color="gray"
          >
            <Popconfirm
              title={t('post-list.button.delete.popConfirm.single-item-title')}
              onConfirm={() => onSingleRecordDeleteBtnClick(record)}
              okText={t('post-list.button.delete.popConfirm.okText')}
              cancelText={t('post-list.button.delete.popConfirm.cancelText')}
              placement={
                localeStore.lang === LanguageActionTypeEnum.en
                  ? 'left'
                  : 'right'
              }
              okButtonProps={{
                danger: true,
              }}
            >
              <Button
                id={record.key}
                onClick={(e) => setTargetBtn(e.currentTarget.id)}
                loading={targetBtn === record.key && postStore.loadingInitial}
                type="text"
                size="middle"
                icon={<DeleteFilled />}
                danger
              />
            </Popconfirm>
          </Tooltip>
        </Space>
      ),
    },
  ];

  const onSingleRecordDeleteBtnClick = async (record: IPostAntdTable) => {
    await postStore.deletePosts([record.key]);
    const existedSelectedRowKeys = selectedRowKeys.filter(
      (value, index, arr) => {
        return value !== record.key;
      }
    );
    setSelectedRowKeys(existedSelectedRowKeys);
    await postStore.loadPosts(DFAULT_PAGE_SIZE, currentPage - 1);
  };

  let data: IPostAntdTable[] = [];

  useEffect(() => {
    if (Array.from(postStore.postRegistry.values()).length === 0) {
      postStore.loadPosts(DFAULT_PAGE_SIZE, 0);
    }

    window.addEventListener('resize', (event) => {
      setWindowWidth(window.innerWidth);
    });

    if (selectedRowKeys.length === 0) {
      setDeleteRangeBtn(true);
    }
  }, [selectedRowKeys.length, postStore]);

  postStore.postRegistry.forEach((post, i) => {
    const ua = new UAParser();
    ua.setUA(post.userAgent);

    /// PostStatus Localization:
    // Localized postStatus: to localize postStatus use "localizedPostStatus" variable as its value
    let localizedPostStatus: any;
    // Setting localized value for postStatus through the following switch statement
    switch (post.postStatus) {
      case PostStatusEnum.Publish:
        localizedPostStatus = t('post-list.table.filters.post-status.publish');
        break;
      case PostStatusEnum.Pending:
        localizedPostStatus = t('post-list.table.filters.post-status.pending');
        break;
      case PostStatusEnum.Inherit:
        localizedPostStatus = t('post-list.table.filters.post-status.inherit');
        break;
      case PostStatusEnum.AutoDraft:
        localizedPostStatus = t(
          'post-list.table.filters.post-status.auto-draft'
        );
        break;
      case PostStatusEnum.Draft:
        localizedPostStatus = t('post-list.table.filters.post-status.draft');
        break;
      case PostStatusEnum.Private:
        localizedPostStatus = t('post-list.table.filters.post-status.private');
        break;
      case PostStatusEnum.Future:
        localizedPostStatus = t('post-list.table.filters.post-status.future');
        break;
      case PostStatusEnum.Trash:
        localizedPostStatus = t('post-list.table.filters.post-status.trash');
        break;
      default:
        localizedPostStatus = '';
        break;
    }
    // Initializing columns data
    data.push({
      key: i,
      title:
        localeStore.lang === LanguageActionTypeEnum.fa ? (
          <Link to={`/admin/posts/details/${i}`}>{e2p(post.title)}</Link>
        ) : (
          <Link to={`/admin/posts/details/${i}`}>{post.title}</Link>
        ),
      postCategory: post.taxonomyPosts.map((taxonomyPost: ITaxonomyPost) => {
        return taxonomyPost.taxonomy.type === TaxonomyTypeEnum.category ? (
          localeStore.lang === LanguageActionTypeEnum.fa ? (
            <div key={i}>{e2p(taxonomyPost.taxonomy.term.name)}</div>
          ) : (
            <div key={i}>{taxonomyPost.taxonomy.term.name}</div>
          )
        ) : (
          ''
        );
      }),
      postStatus: localizedPostStatus,
      postAttachments:
        localeStore.lang === LanguageActionTypeEnum.fa
          ? e2p(post.postAttachments.length.toString())
          : post.postAttachments.length,
      commentAllowed: post.commentAllowed ? (
        <CheckOutlined style={{ color: '#52c41a' }} />
      ) : (
        <CloseOutlined style={{ color: '#f5222d' }} />
      ),
      viewCount:
        localeStore.lang === LanguageActionTypeEnum.fa
          ? e2p(post.viewCount.toString())
          : post.viewCount,
      pinned: post.isPinned ? (
        <CheckOutlined style={{ color: '#52c41a' }} />
      ) : (
        <CloseOutlined style={{ color: '#f5222d' }} />
      ),
      comments:
        localeStore.lang === LanguageActionTypeEnum.fa
          ? e2p(post.comments.toString())
          : post.comments,
      childPosts:
        localeStore.lang === LanguageActionTypeEnum.fa
          ? e2p(post.childPosts.toString())
          : post.childPosts,
      createdAt:
        localeStore.lang === LanguageActionTypeEnum.fa
          ? e2p(
              jalaliMoment(post.createdAt, 'YYYY-MM-DD HH:m:s')
                .locale('fa')
                .format('YYYY-MM-DD HH:m:s')
            )
          : moment(post.createdAt).format('YYYY-MM-DD HH:m:s'),
      createdBy: post.createdBy?.userName,
      modifiedAt: post.modifiedAt
        ? localeStore.lang === LanguageActionTypeEnum.fa
          ? e2p(
              jalaliMoment(post.modifiedAt, 'YYYY-MM-DD HH:m:s')
                .locale('fa')
                .format('YYYY-MM-DD HH:m:s')
            )
          : moment(post.modifiedAt).format('YYYY-MM-DD HH:m:s')
        : '',
      modifiedBy: post.modifiedBy?.userName,
      //userAgent: post.userAgent,
      device: ua.getDevice().type ?? 'Desktop',
      os: `${ua.getOS().name} ${ua.getOS().version}`,
      browser: `${ua.getBrowser().name} ${ua.getBrowser().version}`,
      userIPAddress:
        localeStore.lang === LanguageActionTypeEnum.fa
          ? e2p(post.userIPAddress)
          : post.userIPAddress,
    });
  });

  const confirm = async (e: MouseEvent | undefined) => {
    await postStore.deletePosts(selectedRowKeys as string[]);
    setSelectedRowKeys([]);
    await postStore.loadPosts(DFAULT_PAGE_SIZE, currentPage - 1);
  };

  return (
    <Fragment>
      <Row align="bottom">
        <Col span={12}>
          <Typography>
            <Title level={4}>{t('post-list.title')}</Title>
            <Paragraph ellipsis>
              <Text type="secondary">{t('post-list.text')}</Text>
            </Paragraph>
          </Typography>
        </Col>
        <Col
          span={12}
          style={{
            textAlign:
              localeStore.dir === DirectionActionTypeEnum.LTR
                ? 'right'
                : 'left',
          }}
        >
          <Popconfirm
            title={t('post-list.button.delete.popConfirm.title')}
            onConfirm={confirm}
            okText={t('post-list.button.delete.popConfirm.okText')}
            cancelText={t('post-list.button.delete.popConfirm.cancelText')}
            placement={
              localeStore.lang === LanguageActionTypeEnum.en ? 'left' : 'right'
            }
            okButtonProps={{ danger: true }}
          >
            <Button
              id="delete_range_btn"
              disabled={deleteRangeBtn}
              loading={targetBtn === 'delete_range_btn' && postStore.submitting}
              onClick={(e) => setTargetBtn(e.currentTarget.id)}
              type="primary"
              danger
              icon={<DeleteFilled />}
              size="small"
              style={{ marginBottom: '1rem' }}
            >
              {t('post-list.button.delete.name')}
            </Button>
          </Popconfirm>
        </Col>
      </Row>
      <Row>
        <Table<IPostAntdTable>
          loading={postStore.loadingInitial}
          bordered
          rowSelection={rowSelection}
          columns={columns}
          dataSource={data}
          size="small"
          scroll={{ x: window.innerWidth - 100, y: window.innerHeight - 100 }}
          pagination={{
            size: 'small',
            showSizeChanger: true,
            showQuickJumper: true,
            showTotal: (total, range) => {
              const localizedRangeZero =
                localeStore.lang === LanguageActionTypeEnum.fa
                  ? e2p(range[0].toString())
                  : range[0];
              const localizedRangeOne =
                localeStore.lang === LanguageActionTypeEnum.fa
                  ? e2p(range[1].toString())
                  : range[1];
              const localizedTotal =
                localeStore.lang === LanguageActionTypeEnum.fa
                  ? e2p(total.toString())
                  : total;
              const of = t('post-list.table.pagination.show-total.of');
              const items = t('post-list.table.pagination.show-total.items');
              return `${localizedRangeZero}-${localizedRangeOne} ${of} ${localizedTotal} ${items}`;
            },
            total: postStore.postCount,
            responsive: true,
          }}
          onChange={(pagination, filters, sorter) => {
            const sort = sorter as SorterResult<IPostAntdTable>;
            let filterKey;
            let filterValue;
            let startDate;
            let endDate;
            let startNumber;
            let endNumber;
            for (const [key, value] of Object.entries(filters)) {
              if (value && SEARCH_FILTERED_COLUMNS.includes(key)) {
                filterKey = key;
                filterValue = value[0];
              }
              if (value && DATERANGE_FILTERED_COLUMNS.includes(key)) {
                filterKey = key;
                startDate = value![0];
                endDate = value![1];
              }
              if (value && SLIDER_FILTERED_COLUMNS.includes(key)) {
                filterKey = key;
                startNumber = value[0] as number;
                endNumber = value[1] as number;
              }
            }
            setCurrentPage(pagination.current ? pagination.current! : 1);
            postStore.loadPosts(
              pagination.pageSize,
              pagination.current ? pagination.current! - 1 : undefined,
              filterKey,
              filterValue?.toString(),
              sort.field?.toString(),
              sort.order?.toString(),
              startDate?.toString(),
              endDate?.toString(),
              startNumber,
              endNumber
            );
          }}
        />
      </Row>
    </Fragment>
  );
};

export default withTranslation()(observer(PostList));
